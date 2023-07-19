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
            stateReturn,
            stateAttack,
            stateHit,
            stateDie;

        [Export]
        public MobDetection detection;
        [Export]
        public Vector3 warnOffset = new Vector3(0, 1, 0);
        [Export]
        public float attackDistanceSqr = 25,
            hitDistanceSqr = 0.25f,
            speed = 3.5f,
            offsetSize = 0.33f,
            offsetSpeed = 1f;
        
        public MobFaction enemy;
        public Vector3 startPosition,
            targetPosition;
        public bool useOffset = false;



        public override void _Ready()
        {
            startPosition = GlobalPosition;
            
            // initialize states
            stateIdle = new MobWaspStateIdle(){blackboard = this};
            stateWarn = new MobWaspStateWarn(){blackboard = this};
            stateWarnCooldown = new MobWaspStateWarnCooldown(){blackboard = this};
            stateReturn = new MobWaspStateReturn(){blackboard = this};
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
			if(Engine.TimeScale == 0)
			{
				return;
			}

            // get velocity
            if(useOffset)
            {
                var offset = new Vector3((float) Mathf.Sin(EngineTime.timePassed * offsetSpeed), (float) Mathf.Sin(EngineTime.timePassed * offsetSpeed * 2), 0);
                offset *= offsetSize;
                offset = ToGlobal(offset) - GlobalPosition;

                Velocity = ((targetPosition + offset) - GlobalPosition) * speed;
            }
            else if(GlobalPosition != targetPosition)
            {
                Velocity = (targetPosition - GlobalPosition) * speed;
            }

            // apply movement
            MoveAndSlide();


            if(enemy != null)
            {
                // look   
                LookAt(enemy.GlobalPosition);
            }
        }
    }
}