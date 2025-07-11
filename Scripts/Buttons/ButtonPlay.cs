using Godot;
using System;

public partial class ButtonPlay : Button
{
    public override void _Ready()
    {
        // No need to connect the signal manually
    }

    private void _on_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/level.tscn");
        Visible = false;
    }
}
