using Godot;
using System;

public class StateMachineQueue : StateMachine 
{

	



	public void Enable()
	{
		StateMachineQueuePopper.machines.Add(this);
	}



    public void Disable()
    {
        StateMachineQueuePopper.machines.Remove(this);
    }



    void Start()
	{
		//currentState.StartState();
	}



	public void RunMachine(double delta)
	{
		if(currentState != null)
		{
			currentState.RunState(delta);
		}

		SetState(currentState.Transition());
	}
}
