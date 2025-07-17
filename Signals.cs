using Godot;
using System;
//using System.Numerics;

public partial class Signals : Node
{
    [Signal]
    public delegate void PlayerPositionUpdateEventHandler(Vector2 position);

    public void UpdatePlayerPosition(Vector2 newPosition)
    {
        // 2. Испускание сигнала (правильный способ)
        EmitSignal(SignalName.PlayerPositionUpdate, newPosition);
        
        // Альтернативный вариант:
        // EmitSignal(nameof(PlayerPositionUpdateEventHandler), newPosition);
    }
}
