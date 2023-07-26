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
        public AnimationTree animation;
        [Export]
        public GibsActivator gibsActivator;
        [Export]
        public AudioTools3d audio;
        [Export]
        public AudioStream flySound;
        [Export]
        public PackedScene waspDeathFx;
        [Export]
        public string targetName = "Wasp",
            arrowType = "weighted";
        [Export]
        public Vector3 warnOffset = new Vector3(0, 1, 0);
        [Export]
        public float maxRangeForEnemies = 100,
            maxRangeForAllies = 16,
            attackDistanceSqr = 25,
            hitDistanceSqr = 0.5f,
            maxFlightRangeSqr = 1600,
            speed = 3.5f,
            acceleration = 4,
            offsetSize = 0.33f,
            offsetSpeed = 1f;
        
        public MobFaction enemy;
        public List<MobFaction> allies = new List<MobFaction>();
        public Vector3 startPosition,
            targetPosition,
            lookTargetPosition;
        public double offsetCursor = 0;
        public int startingAllyCount;
        public bool useOffset = false,
            updateLook = false,
            allyDied = false;

        int offsetDirection = 1;




        public override void _Ready()
        {
            startPosition = GlobalPosition;
            targetPosition = startPosition;
            lookTargetPosition = GlobalPosition + -Basis.Z;

            if(GD.Randi() % 2 == 1)
            {
                offsetDirection = -1;
            }

            offsetSpeed *= 1 + (GD.Randf() - 0.5f) * 0.2f;
            
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
        }



        public override void _EnterTree()
        {
            machine.Enable();
        }



        public override void _ExitTree()
        {
            machine.Disable();
        }



        public override void _PhysicsProcess(double delta)
        {
            var moveTarget = targetPosition;

            // check if using offset
            if(useOffset)
            {
                offsetCursor += delta * offsetSpeed * offsetDirection;

                // use figure 8 offset
                var timeCursorHorizontal = offsetCursor;
                var timeCursorVertical = offsetCursor * 2;
                var offset = new Vector3((float) Mathf.Sin(timeCursorHorizontal), (float) Mathf.Sin(timeCursorVertical), 0);
                offset *= offsetSize;
                offset = ToGlobal(offset) - GlobalPosition;

                moveTarget = targetPosition + offset;
            }
            
            // check if not close to move target
            if(GlobalPosition.DistanceSquaredTo(moveTarget) > 0.01f)
            {
                // move to target
                var newVelocity = (moveTarget - GlobalPosition).Normalized() * speed;

                if(Velocity != newVelocity)
                {
                    Velocity = Velocity.Lerp(newVelocity, acceleration * ((float) delta));
                }  

                // apply movement
                MoveAndSlide();
            }
            else
            {
                // smooth velocity
                Velocity = Velocity.Lerp(Vector3.Zero, acceleration * ((float) delta));

                // snap to target
                GlobalPosition = moveTarget;
            }

            // animation fly blend using speed
            animation.Set("parameters/Fly/blend_position", Velocity.LengthSquared() / Mathf.Pow(speed, 2));


            if(enemy != null)
            {
                // flatten look direction
                var target = enemy.GlobalPosition;
                target.Y = GlobalPosition.Y;

                if(target != GlobalPosition)
                {
                    // smooth look target
                    lookTargetPosition = lookTargetPosition.Lerp(target, speed * ((float) delta));  

                    // look at enemy
                    LookAt(lookTargetPosition);
                }
            }
            else if(updateLook && Velocity != Vector3.Zero)
            {
                // flatten look direction
                var target = GlobalPosition + Velocity;
                target.Y = GlobalPosition.Y;

                // smooth look target
                lookTargetPosition = lookTargetPosition.Lerp(target, speed * ((float) delta));  

                // look in direction of movement
                LookAt(lookTargetPosition);
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