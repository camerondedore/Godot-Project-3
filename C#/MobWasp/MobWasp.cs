using Godot;
using System;

namespace MobWasp
{
    public partial class MobWasp : CharacterBody3D
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateIdle,
            stateWarn,
            stateWarnCooldown,
            stateAttack,
            stateHit,
            stateDie;

        [Export]
        public MobDetection detection;
        [Export]
        public float warnDistanceSqr = 100,
            attackDistanceSqr = 25,
            hitDistanceSqr = 0.25f;
        
        public MobFaction enemy;
        public Vector3 startPosition;



        public override void _Ready()
        {
            startPosition = GlobalPosition;
            
            // initialize states
            stateIdle = new MobWaspStateIdle(){blackboard = this};
            stateWarn = new MobWaspStateWarn(){blackboard = this};
            stateWarnCooldown = new MobWaspStateWarnCooldown(){blackboard = this};
            stateAttack = new MobWaspStateAttack(){blackboard = this};
            stateHit = new MobWaspStateHit(){blackboard = this};
            stateDie = new MobWaspStateDie(){blackboard = this};

            // set first state in machine
            machine.SetState(stateIdle);

            machine.Enable();
        }



        // public override void _PhysicsProcess(double delta)
        // {
        //     time check
        //     if(Engine.TimeScale == 0)
        //     {
        //         return;
        //     }

        //     run machine
        //     if(machine != null && machine.CurrentState != null)
        //     {
        //         machine.CurrentState.RunState(delta);
        //         machine.SetState(machine.CurrentState.Transition());
        //     }
        // }
    }
}