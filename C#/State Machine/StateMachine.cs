using System.Collections;
using System.Collections.Generic;
using System;
using Godot;

public class StateMachine 
{

	public State CurrentState
	{
		get
		{
			return currentState;
		}
	}
	public bool frozen = false;
	protected State currentState;



	public void SetState(State nextState)
	{
		// if machine is frozen
		if(frozen)
		{
			return;
		}

		// if next state is not the current state
		if(nextState != currentState)
		{
			if(currentState != null)
			{
				// end current state
				currentState.EndState();			
			}

			// transition to next state
			currentState = nextState;

			if(currentState != null)
			{
				// start new state
				currentState.StartState();
			}
		}
	}
}
