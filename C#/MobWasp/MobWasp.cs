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
        public GpuParticles3D venomFx;
        [Export]
        public AudioTools3d flyAudio;
        [Export]
        public AudioStream flySound;
        [Export]
        public PackedScene waspDeathFx,
            waspHitFx;
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
            lookSpeed = 4,
            acceleration = 4,
            offsetSize = 0.33f,
            offsetSpeed = 1f;
        
        public MobFaction enemy;
        public Vector3 targetPosition,
            startPosition;
        public double offsetCursor = 0;
        public bool useOffset = false,
            lookWithVelocity = false,
            allyDied = false;

        Vector3 startForward,
            startUp,
            lookTargetPosition;
        int offsetDirection = 1;




        public override void _Ready()
        {
            startPosition = GlobalPosition;
            startForward = -Basis.Z;
            startUp = Basis.Y;
            targetPosition = startPosition;
            lookTargetPosition = startPosition + startForward;

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
            if(GlobalPosition.DistanceSquaredTo(moveTarget) > 0.0225f)
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
            var flatVelocity = Velocity;
            flatVelocity.Y = 0;
            var flyBlend = flatVelocity.LengthSquared() / Mathf.Pow(speed, 2);
            animation.Set("parameters/Fly/blend_position", flyBlend);


            var target = Vector3.Zero;
            var upDirection = Vector3.Up;

            if(enemy != null)
            {
                // look at enemy
                target = (enemy.GlobalPosition - GlobalPosition).Normalized() + GlobalPosition;
                target.Y = GlobalPosition.Y;
            }
            else if(lookWithVelocity)
            {
                if(flatVelocity.LengthSquared() > 0.4f)
                {
                    // look in direction of movement
                    target = GlobalPosition + flatVelocity.Normalized();
                }
            }
            else if(machine.CurrentState == stateIdle)
            {
                // starting look
                target = startPosition + startForward;
                upDirection = startUp;
            }


            if(target != Vector3.Zero && target != GlobalPosition)
            {
                // smooth look target
                lookTargetPosition = lookTargetPosition.Lerp(target, lookSpeed * ((float) delta));  

                // look at target
                LookAt(lookTargetPosition, upDirection);
            }
            else
            {                
                // get a flattened forward
                target = GlobalPosition + -Basis.Z;
                target.Y = GlobalPosition.Y;
                
                // smooth look target
                lookTargetPosition = lookTargetPosition.Lerp(target, lookSpeed * ((float) delta));
                //lookTargetPosition = target;

                // check angle from forward to up
                if(Mathf.Abs(-Basis.Z.AngleTo(upDirection)) > 0.0017f)
                {
                    // look forward
                    LookAt(lookTargetPosition, upDirection);
                }
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