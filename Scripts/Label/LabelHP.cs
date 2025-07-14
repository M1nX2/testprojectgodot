using Godot;
using System;

public partial class LabelHP : Label
{
	Player player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        player = GetNode<Player>("/root/Level/Player/Player");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = "HP: " + player.Health.ToString();
	}
}
