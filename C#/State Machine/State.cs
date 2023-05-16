using System.Collections;
using System.Collections.Generic;
using System;
using Godot;

public abstract class State 
{

	// states need to have their blackboard added in their derived class
	// ex:
	// public PlayerState
	// {
	// 	  public PlayerBlackboard blackboard;
	// }
	//
	// states are stored and initialized on the appropriate blackboard
	// ex:
	//public MobBlackboard : Monobehavior
	//  public StateMachine machine;
	//  public State stateAttack; 
	//  
	//  void Awake()
	//  {
	// 	  stateAttack = new PlayerState(){blackboard = this};
	// 	  machine.SetState(stateAttack);
	//  }



	public virtual void RunState(double delta)
	{

	}



	public virtual void StartState()
	{

	}



	public virtual void EndState()
	{

	}



	public virtual State Transition()
	{
		return this;
	}

}
