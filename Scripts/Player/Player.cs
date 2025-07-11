using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -400.0f;

	public int gold = 0;
	public int health = 100;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private AnimatedSprite2D anim;

    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

	public override void _Process(double delta)
	{ 
		if (health <= 0	) 
		{ 
			QueueFree(); 
			GetTree().ChangeSceneToFile("res://Scenes/menu.tscn");
		}
	}

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = base.Velocity;

		// Add the gravity.
		if (!base.IsOnFloor())
		{
			velocity += base.GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && base.IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			anim.Play("Jump");
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			if (velocity.Y == 0)
				anim.Play("Run");
		}
		else
		{
			velocity.X = Mathf.MoveToward(base.Velocity.X, 0, Speed);
            if (velocity.Y == 0)
                anim.Play("Idle");
		}

		if (direction.X == -1) 
		{
			anim.FlipH = true;
		}
        else if (direction.X == 1)
        {
			anim.FlipH = false;
        }

		if (velocity.Y > 0)
        {
            anim.Play("Fall");
        }

        base.Velocity = velocity;
		base.MoveAndSlide();
	}
}
