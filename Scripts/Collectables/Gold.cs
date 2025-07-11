using Godot;
using System;

public partial class Gold : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void _on_body_entered(Node2D body)
    {
        if (body is Player)
        {
			Tween tween1 = GetTree().CreateTween();
			Tween tween2 = GetTree().CreateTween();

            tween1.TweenProperty(this, "position", Position - new Vector2(0, 25), 0.3f);
            //tween2.TweenProperty(this, "modulate", new Color(1, 1, 1, 0), 0.3f);
            tween2.TweenProperty(this, "modulate:a", 0, 0.3f);
			          
            tween1.TweenCallback(Callable.From(() => QueueFree()));

            Player player = (Player)body;
			player.gold += 1;
        }
    }
}
