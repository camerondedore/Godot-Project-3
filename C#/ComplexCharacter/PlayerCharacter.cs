using Godot;
using System;

public partial class PlayerCharacter : CharacterBody3D
{

	public StateMachine machine = new StateMachine();
	public State stateStart,
		stateIdle;

	[Export]
	float maxSlopeAngle,
		maxSlideAngle;

	public float maxSlopeAngleRad,
		maxSlideAngleRad;

    string debugText;
	



	public override void _Ready()
	{
		// time check
		if(Engine.TimeScale == 0)
		{
			return;
		}

		// get max angles in radians
		maxSlopeAngleRad = Mathf.DegToRad(maxSlopeAngle);
		maxSlideAngleRad = Mathf.DegToRad(maxSlideAngle);

		// initialize states
		stateStart = new PlayerCharacterStateStart(){blackboard = this};
		stateIdle = new PlayerCharacterStateIdle(){blackboard = this};

		// set first state in machine
		machine.SetState(stateStart);
	}



    public override void _PhysicsProcess(double delta)
    {
        // run machine
		if(machine != null && machine.CurrentState != null)
		{
			machine.CurrentState.RunState(delta);
			machine.SetState(machine.CurrentState.Transition());
		}


        // debug
		if(debugText != machine.CurrentState.ToString())
		{
			GD.Print(machine.CurrentState.ToString());
			debugText = machine.CurrentState.ToString();
		}   
		//GD.Print(velocity.Length());
		//GD.Print(jumpStartY + " : " + fallStartY); 
    }
}
