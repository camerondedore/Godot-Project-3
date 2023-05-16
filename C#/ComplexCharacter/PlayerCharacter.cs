using Godot;
using System;

public partial class PlayerCharacter : CharacterBody3D
{

    public StateMachine machine = new StateMachine();
    public State idleState;



    public override void _Ready()
	{
        // time check
		if(Engine.TimeScale == 0)
		{
			return;
		}
    }
}