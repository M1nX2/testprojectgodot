using Godot;
using System;

public partial class ButtonQuit : Button
{
    private void _on_pressed()
    {
        GetTree().Quit();
        Visible = false;
    }
}
