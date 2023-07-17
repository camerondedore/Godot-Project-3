using Godot;
using System;

namespace MobWasp
{
    public partial class MobWasp : CharacterBody3D
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateIdle,
            stateWarn,
            stateAttack,
            stateHit,
            stateDie;

        [Export]
        public MobDetection detection;
        [Export]
        public float warnDistance = 10,
            attackDistance = 5,
            hitDistance = 0.5f;



        public override void _Ready()
        {

            // initialize states
            stateIdle = new MobWaspStateIdle(){blackboard = this};
            stateWarn = new MobWaspStateWarn(){blackboard = this};
            stateAttack = new MobWaspStateAttack(){blackboard = this};
            stateHit = new MobWaspStateHit(){blackboard = this};
            stateDie = new MobWaspStateDie(){blackboard = this};

            // set first state in machine
            machine.SetState(stateIdle);

            machine.Enable();
        }



        public override void _PhysicsProcess(double delta)
        {
            // time check
            // if(Engine.TimeScale == 0)
            // {
            //     return;
            // }

            // run machine
            // if(machine != null && machine.CurrentState != null)
            // {
            //     machine.CurrentState.RunState(delta);
            //     machine.SetState(machine.CurrentState.Transition());
            // }


            // debug
            // if(debugText != machine.CurrentState.ToString())
            // {
            // 	GD.Print(machine.CurrentState.ToString());
            // 	debugText = machine.CurrentState.ToString();
            // }   
        }
    }
}