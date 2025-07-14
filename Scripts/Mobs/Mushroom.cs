using Godot;
using System;

public partial class Mushroom : CharacterBody2D
{
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
        animPlayer.Play("Attack");
    }
}
