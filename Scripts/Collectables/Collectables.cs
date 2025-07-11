using Godot;
using System;

public partial class Collectables : Node2D
{
	private PackedScene gold;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gold = GD.Load<PackedScene>("res://Scenes/Collectables/Gold.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_timer_timeout()
    {
		Node2D goldTemp = (Node2D)gold.Instantiate();
		RandomNumberGenerator rng = new RandomNumberGenerator();
		int randint = rng.RandiRange(60, 500);
		goldTemp.Position = new Vector2(randint, 950);
        AddChild(goldTemp);
    }
}
