using Godot;
using System;

public partial class Enemy : Node2D
{
    int speed = 260;

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {
        //movement
        Vector2 movement = new Vector2();
        movement.Y += speed * (float)delta;
        Position += movement;
        //check if the enemy is out of the screen
        if (Position.Y > GetViewportRect().Size.Y)
        {
            QueueFree();
        }

    }
}
