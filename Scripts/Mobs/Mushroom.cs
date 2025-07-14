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

    private StateType state;

    public StateType State
    {
        get { return state; }
        set 
        { 
            state = value; 
            switch (state)
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
            }
        }
    }

    private Vector2 velocity = Vector2.Zero;


    AnimationPlayer animPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
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
    }
}
