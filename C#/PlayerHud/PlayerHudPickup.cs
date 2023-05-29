using Godot;
using System;

public partial class PlayerHudPickup : Control
{

    public Vector2 endPosition;

    Vector2 startPosition;
    float speed = 2.5f,
        lerpIndex = 0;



    public override void _Ready()
    {
        startPosition = Position;
    }



    public override void _Process(double delta)
    {
        Position = startPosition.Lerp(endPosition, lerpIndex);

        lerpIndex += ((float)delta) * speed;

        if(lerpIndex >= 1)
        {
            QueueFree();
        }
    }
}