using Godot;
using System;
using System.Collections.Generic;

namespace MobWasp
{
    public partial class MobWasp : CharacterBody3D, IBowTarget
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateIdle,
            stateIdleAlert,
            stateTakeoff,
            stateLand,
            stateWarn,
            stateCooldown,
            stateReturn,
            stateAttack,
            stateHit,
            stateDie;

        [Export]
        public MobDetection detection;
        [Export]
        public string targetName = "Wasp",
            arrowType = "weighted";
        [Export]
        public Vector3 warnOffset = new Vector3(0, 1, 0);
        [Export]
        public float maxRangeForEnemies = 144,
            maxRangeForAllies = 16,
            attackDistanceSqr = 25,
            hitDistanceSqr = 0.25f,
            speed = 3.5f,
            acceleration = 4,
            offsetSize = 0.33f,
            offsetSpeed = 1f;
        
        public MobFaction enemy;
        public List<MobFaction> allies = new List<MobFaction>();
        public Vector3 startPosition,
            targetPosition;
        public int startingAllyCount;
        public bool useOffset = false,
            updateLook = false,
            allyDied = false;



        public override void _Ready()
        {
            startPosition = GlobalPosition;
            
            // initialize states
            stateIdle = new MobWaspStateIdle(){blackboard = this};
            stateIdleAlert = new MobWaspStateIdleAlert(){blackboard = this};
            stateTakeoff = new MobWaspStateTakeoff(){blackboard = this};
            stateLand = new MobWaspStateLand(){blackboard = this};
            stateWarn = new MobWaspStateWarn(){blackboard = this};
            stateCooldown = new MobWaspStateCooldown(){blackboard = this};
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
            
            var newVelocity = Velocity;

            // get velocity
            if(useOffset)
            {
                // use figure 8 offset
                var offset = new Vector3((float) Mathf.Sin(EngineTime.timePassed * offsetSpeed), (float) Mathf.Sin(EngineTime.timePassed * offsetSpeed * 2), 0);
                offset *= offsetSize;
                offset = ToGlobal(offset) - GlobalPosition;

                var vectorToTarget = (targetPosition + offset) - GlobalPosition;
                var disanceToTarget = vectorToTarget.Length();

                // clamp speed using next tic's distance traveled
                var clampedSpeed = Mathf.Clamp(speed * ((float) delta), 0, disanceToTarget) / ((float) delta);

                newVelocity = vectorToTarget.Normalized() * clampedSpeed;
            }
            else if(GlobalPosition != targetPosition)
            {
                // normal movement
                var vectorToTarget = targetPosition - GlobalPosition;
                var disanceToTarget = vectorToTarget.Length();
                
                // clamp speed using next tic's distance traveled
                var clampedSpeed = Mathf.Clamp(speed * ((float) delta), 0, disanceToTarget) / ((float) delta);

                newVelocity = vectorToTarget.Normalized() * clampedSpeed;
            }

            Velocity = Velocity.Lerp(newVelocity, acceleration * ((float) delta));

            // apply movement
            MoveAndSlide();


            if(enemy != null)
            {
                // flatten look direction
                var target = enemy.GlobalPosition;
                target.Y = GlobalPosition.Y;

                // look at enemy
                LookAt(target);
            }
            else if(updateLook && Velocity != Vector3.Zero)
            {
                // flatten look direction
                var target = GlobalPosition + Velocity;
                target.Y = GlobalPosition.Y;

                // look in direction of movement
                LookAt(target);
            }
        }



        public string GetName()
        {
            return targetName;
        }



        public string GetArrowType()
        {
            return arrowType;
        }



        public Vector3 GetGlobalPosition()
        {
            try
            {
                return GlobalPosition;
            }
            catch
            {
                // target was disposed
                // nothing more to do
                return Vector3.Zero;
            }
        }



        public void Hit()
        {
            // wasp is dead
            machine.SetState(stateDie);

            // disable script
            //SetScript(new Variant());
        }
    }
}