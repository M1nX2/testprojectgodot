using Godot;
using System;

public partial class Mushroom : CharacterBody2D
{
    public enum StateType
    { 
        IDLE,
        ATTACK,
        CHASE
    }

    private StateType _state;

    private Signals signals;

    public StateType State
    {
        get { return _state; }
        set 
        { 
            _state = value; 
            switch (_state)
            {
                case StateType.IDLE:
                    IdleStateStart();
                    break;
                case StateType.ATTACK:
                    AttackStateStart();
                    break;
                case StateType.CHASE:
                    ChaseStateStart();
                    break;
                default:
                    break;
            }
        }
    }

    private Vector2 velocity = Vector2.Zero;
    private Vector2 playerPosition = Vector2.Zero;
    private Vector2 direction = Vector2.Zero;

    private Node2D attackDirection;


    private AnimationPlayer animPlayer;
    private AnimatedSprite2D sprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        signals = GetNode<Signals>("/root/Signals");
        attackDirection = GetNode<Node2D>("AttackDirection");

        Callable callable = new(this, "_on_player_position_update");
        signals.Connect("PlayerPositionUpdate", callable);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta) 
	{
        velocity = base.Velocity;

        // Add the gravity.
        if (!base.IsOnFloor())
        {
            velocity += base.GetGravity() * (float)delta;
        }
        MoveAndSlide();
        base.Velocity = velocity;
    }

    private void _on_player_position_update(Vector2 position)
    {
        playerPosition = position;

        if (position.X < Position.X)
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, 100);
        }
        else
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, -100);
        }
    }

    public void _on_attack_range_body_entered(Node2D body)
    {
        State = StateType.ATTACK;
    }

    private void IdleStateStart()
    { 
        animPlayer.Play("Idle");
    }

    private async void AttackStateStart()
    {
        animPlayer.Play("Attack");
        await ToSignal(animPlayer, "animation_finished");
        GetNode<CollisionShape2D>("AttackDirection/AttackRange/CollisionShape2D").Disabled = true;
        State = StateType.CHASE;
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        GetNode<CollisionShape2D>("AttackDirection/AttackRange/CollisionShape2D").Disabled = false;
    }

    private void ChaseStateStart()
    {
        animPlayer.Play("Run");
        direction = (playerPosition - this.Position).Normalized();
        if (direction.X > 0)
        {
            sprite.FlipH = false;
            attackDirection.RotationDegrees = 0;
        }
        else
        {
            sprite.FlipH = true;
            attackDirection.RotationDegrees = 180;
        }
    }
}
