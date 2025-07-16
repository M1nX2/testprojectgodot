using Godot;
using System;

public partial class bg : ParallaxBackground
{
	private double speed = 100;

	public override void _Process(double delta)
	{
		ScrollOffset -= new Vector2((float)speed * (float)delta, 0);
	}
}
