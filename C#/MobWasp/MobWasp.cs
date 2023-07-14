using Godot;
using System;

public partial class MobWasp : CharacterBody3D
{

    public StateMachine machine = new StateMachine();
    public State stateIdle,
        stateWarn,
        stateAttack,
        stateHit,
        stateDie;



    public override void _Ready()
    {

        // initialize states
        // stateIdle = new MobWaspState(){blackboard = this};
        // stateWarn = new MobWaspState(){blackboard = this};
        // stateAttack = new MobWaspState(){blackboard = this};
        // stateHit = new MobWaspState(){blackboard = this};
        // stateDie = new MobWaspState(){blackboard = this};

        // set first state in machine
        machine.SetState(stateIdle);
    }



    public override void _PhysicsProcess(double delta)
    {
        // time check
        if(Engine.TimeScale == 0)
        {
            return;
        }

        // run machine
        if(machine != null && machine.CurrentState != null)
        {
            machine.CurrentState.RunState(delta);
            machine.SetState(machine.CurrentState.Transition());
        }


        // debug
        // if(debugText != machine.CurrentState.ToString())
        // {
        // 	GD.Print(machine.CurrentState.ToString());
        // 	debugText = machine.CurrentState.ToString();
        // }   
    }
}
