using System.Collections;
using System.Collections.Generic;
using System;
using Godot;

public class SuperState : State 
{

	// states need to have their blackboard added in their derived class
	// ex:
	// public PlayerSuperState
	// {
	// 	  public PlayerBlackboard blackboard;
	// }

	protected State subState;


	/// <summary>
	/// Runs by calling sub state's run. 
	/// Not needed on derived high states.
	/// </summary>
	public override void RunState(double delta)
	{
		// run sub state
		subState.RunState(delta);

		// check sub state for transitions
		State nextState = subState.Transition();

		// transition substate
		SetState(nextState);
	}



	/// <summary>
	/// Starts by calling sub state's start. 
	/// Not needed on derived high states.
	/// </summary>
	public override void StartState()
	{
		// initialize
		subState.StartState();
	}



	/// <summary>
	/// Ends by calling sub state's end. 
	/// Not needed on derived high states.
	/// </summary>
	public override void EndState()
	{
		// stop
		subState.EndState();
	}



	public void SetState(State nextState)
	{
		// if next state is not the current state
		if(nextState != subState)
		{
			if(subState != null)
			{
				// end current state
				subState.EndState();			
			}

			// transition to next state
			subState = nextState;

			if(subState != null)
			{
				// start new state
				subState.StartState();
			}
		}
	}
}
