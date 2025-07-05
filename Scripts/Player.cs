using Godot;
using System;

public partial class Player : Node2D
{
	[Export] int speed = 260;

	public override void _Ready()
	{
		
	}

	public void MovementControl(double delta)
	{
        //set a new vector

        Vector2 movement = new Vector2();

        //change vector according to WASD
        if (Input.IsKeyPressed(Key.W))
        {
            movement.Y = movement.Y - speed * (float)delta;
        }

        if (Input.IsKeyPressed(Key.A))
        {
            movement.X = movement.X - speed * (float)delta;
        }

        if (Input.IsKeyPressed(Key.S))
        {
            movement.Y = movement.Y + speed * (float)delta;
        }

        if (Input.IsKeyPressed(Key.D))
        {
            movement.X = movement.X + speed * (float)delta;
        }

        //increment the position with the vector
        Position += movement;
    }

	public override void _Process(double delta)
	{
        //check that Player dont hit Enemy hitbox


		MovementControl(delta);


	}
}
