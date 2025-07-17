using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Player : CharacterBody2D
{
	enum StateType
    {
        IDLE,
		MOVE,
		ATTACK1,
		ATTACK2,
		ATTACK3,
		BLOCK,
		SLIDE,
        JUMP,
        FALL
    }

	StateType state = StateType.MOVE;


    public int RunSpeed = 1;
	public const float Speed = 200.0f;
	public const float JumpVelocity = -400.0f;

	public int Gold { get; set; }
    public int Health { get; set; }

    private bool combo = false;
    private bool attackCooldown = false;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private AnimatedSprite2D anim;
    private AnimationPlayer animPlayer;

    private Signals signals;

    private Vector2 velocity = Vector2.Zero;
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        signals = GetNode<Signals>("/root/Signals");

        Health = 100;
        Gold = 0;
    }

	public override void _Process(double delta)
	{ 

	}

    public override async void _PhysicsProcess(double delta)
	{
        velocity = base.Velocity;

        switch (state) 
		{
            case StateType.IDLE:
                break;
            case StateType.MOVE:
                MoveState();
                break;
            case StateType.ATTACK1:
                AttackState();
                break;
            case StateType.ATTACK2:
                Attack2State();
                break;
            case StateType.ATTACK3:
                Attack3State();
                break;
            case StateType.BLOCK:
                BlockState();
                break;
            case StateType.SLIDE:
                SlideState();
                break;
            case StateType.JUMP:
                break;
            case StateType.FALL:
                break;
		}

        // Add the gravity.
        if (!base.IsOnFloor())
		{
			velocity += base.GetGravity() * (float)delta;
		}



        if (Health <= 0)
        {
            Health = 0;
            animPlayer.Play("Death");
            await ToSignal(animPlayer, "animation_finished");
            QueueFree();
            GetTree().ChangeSceneToFile("res://Scenes/menu.tscn");
        }

        if (velocity.Y > 0)
        {
            animPlayer.Play("Fall");
        }

        base.Velocity = velocity;
		base.MoveAndSlide();

        signals.EmitSignal("PlayerPositionUpdate", this.Position);
	}

    private void MoveState()
    {
        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        float direction = Input.GetAxis("left", "right");
        if (direction != 0)
        {
            velocity.X = direction * Speed * RunSpeed;
            if (velocity.Y == 0)
            {
                if (RunSpeed == 1)
                    animPlayer.Play("Walk");
                else
                    animPlayer.Play("Run");
            }               
        }
        else
        {
            velocity.X = Mathf.MoveToward(base.Velocity.X, 0, Speed);
            if (velocity.Y == 0)
                animPlayer.Play("Idle");
        }

        if (direction == -1)
        {
            anim.FlipH = true;
        }
        else if (direction == 1)
        {
            anim.FlipH = false;
        }

        if (Input.IsActionPressed("run") && base.IsOnFloor())
        {
            RunSpeed = 2;
        }
        else 
        {
            RunSpeed = 1;
        }

        if (Input.IsActionPressed("block"))
        {
            if (velocity.X==0)
            state = StateType.BLOCK;
            else
            state = StateType.SLIDE;
        }

        if (Input.IsActionJustPressed("attack") && !attackCooldown)
        { 
            state = StateType.ATTACK1;
        }

    }

    private void BlockState()
    {
        animPlayer.Play("Block");
        if (Input.IsActionJustReleased("block"))
        {
            state = StateType.MOVE;
        }
    }

    private async void SlideState()
    {
        animPlayer.Play("Slide");
        await ToSignal(animPlayer, "animation_finished");
        state = StateType.MOVE;        
    }

    private async void AttackState()
    {
        if (Input.IsActionJustPressed("attack") && combo == true && !attackCooldown)
        {
            state = StateType.ATTACK2;
        }
        velocity.X = 0;
        
        animPlayer.Play("Attack");
        await ToSignal(animPlayer, "animation_finished");
        AttackFreeze();
        state = StateType.MOVE;
    }

    public async void Attack2State()
    {
        if (Input.IsActionJustPressed("attack") && combo == true && !attackCooldown)
        {
            state = StateType.ATTACK3;
        }
        animPlayer.Play("Attack2");
        await ToSignal(animPlayer, "animation_finished");
        state = StateType.MOVE;
    }

    public async void Attack3State()
    {
        animPlayer.Play("Attack3");
        await ToSignal(animPlayer, "animation_finished");
        state = StateType.MOVE;
    }

    public async void Combo1()
    {
        combo = true;
        await ToSignal(animPlayer, "animation_finished");
        combo = false;
    }

    private async void AttackFreeze()
    {
        attackCooldown = true;
        await ToSignal(GetTree().CreateTimer(0.5), "timeout");
        attackCooldown = false;
    }

}
