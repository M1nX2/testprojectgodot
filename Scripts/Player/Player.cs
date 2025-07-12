using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Player : CharacterBody2D
{
	enum State
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

	State state = State.MOVE;


    public int RunSpeed = 1;
	public const float Speed = 200.0f;
	public const float JumpVelocity = -400.0f;

	public int gold = 0;
	public int health = 100;

    private int combo = 0;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private AnimatedSprite2D anim;
    private AnimationPlayer animPlayer;

    private Vector2 velocity = Vector2.Zero;

    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

	public override void _Process(double delta)
	{ 

	}

    public override async void _PhysicsProcess(double delta)
	{
        velocity = base.Velocity;

        switch (state) 
		{
            case State.IDLE:
                break;
            case State.MOVE:
                MoveState();
                break;
            case State.ATTACK1:
                AttackState();
                break;
            case State.ATTACK2:
                break;
            case State.ATTACK3:
                break;
            case State.BLOCK:
                BlockState();
                break;
            case State.SLIDE:
                SlideState();
                break;
            case State.JUMP:
                break;
            case State.FALL:
                break;
		}

        // Add the gravity.
        if (!base.IsOnFloor())
		{
			velocity += base.GetGravity() * (float)delta;
		}



        if (health <= 0)
        {
            health = 0;
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
            state = State.BLOCK;
            else
            state = State.SLIDE;
        }

        if (Input.IsActionJustPressed("attack"))
        { 
            state = State.ATTACK1;
        }

    }

    private void BlockState()
    {
        animPlayer.Play("Block");
        if (Input.IsActionJustReleased("block"))
        {
            state = State.MOVE;
        }
    }

    private async void SlideState()
    {
        animPlayer.Play("Slide");
        await ToSignal(animPlayer, "animation_finished");
        state = State.MOVE;        
    }

    private async void AttackState()
    {
        velocity.X = 0;
        
        animPlayer.Play("Attack");
        await ToSignal(animPlayer, "animation_finished");
        state = State.MOVE;
    }

    public async void Combo1(AnimationPlayer animationPlayer)
    {
        combo = 1;
        await ToSignal(animPlayer, "animation_finished");
        combo = 0;
    }
}
