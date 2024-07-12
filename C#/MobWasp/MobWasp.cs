using Godot;
using System;
using PlayerBow;

namespace MobWasp
{
    public partial class MobWasp : CharacterBody3D, IBowTarget, IMobAlly
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
        public PackedScene waspDeathFx,
            waspHitFx;

        
        string arrowType = "weighted";
        public MobDetection detection;
        public MobEyes eyes;
        public Health health;
        public AnimationTree animation;
        public GibsActivator gibsActivator;
        public GpuParticles3D venomFx;
        public AudioTools3d audio;
        public MobFaction enemy;
        public AnimationNodeStateMachinePlayback animStateMachinePlayback;
        public AudioStream flySound;
        public Vector3 targetPosition,
            startPosition,
            warnOffset = new Vector3(0, 1, 0);
        public double offsetCursor = 0;
        public float maxSightRangeSqr = 100,
            maxSightRangeForAlliesSqr = 16,
            attackDistanceSqr = 25,
            hitDistanceSqr = 0.5f,
            maxFlightRangeSqr = 1600,
            speed = 3.5f,
            lookSpeed = 5,
            acceleration = 4,
            offsetSize = 0.33f,
            offsetSpeed = 1f,
            damage = 2;
        public bool useOffset = false,
            lookWithVelocity = false,
            isAggro = false;

        Vector3 startForward,
            startUp,
            lookTargetPosition;
        int offsetDirection = 1;




        public override void _Ready()
        {
            // get nodes
            detection = (MobDetection) GetNode("Detection");
            eyes = (MobEyes) GetNode("Eyes");
            health = (Health) GetNode("Health");
            animation = (AnimationTree) GetNode("AnimationTree");
            gibsActivator = (GibsActivator) GetNode("Gibs");
            venomFx = (GpuParticles3D) GetNode("FxWaspVenom");
            audio = (AudioTools3d) GetNode("Audio");

            // get flying sound from audio source
            flySound = audio.Stream;

            startPosition = GlobalPosition;
            startForward = -Basis.Z;
            startUp = Basis.Y;
            targetPosition = startPosition;
            lookTargetPosition = startPosition + startForward;

            animStateMachinePlayback = (AnimationNodeStateMachinePlayback) animation.Get("parameters/playback");

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
            animation.Set("parameters/wasp-fly/blend_position", flyBlend);

            // pitch flying audio
            audio.PitchScale = 1 + flyBlend * 0.3f;


            var target = Vector3.Zero;
            var upDirection = Vector3.Up;

            if(IsEnemyValid())
            {
                // look at enemy
                target = enemy.GlobalPosition;
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



        public string GetArrowType()
        {
            return arrowType;
        }



        public Vector3 GetGlobalPosition()
        {
            if(IsInstanceValid(this))
            {
                return GlobalPosition;
            }

            return Vector3.Zero;
        }



        public void Hit(Vector3 dir)
        {
            // wasp is dead
            machine.SetState(stateDie);
        }



        public void LookForEnemy()
        {
            // var newEnemy = detection.LookForEnemy(lookDistanceSqr);
            var newEnemy = detection.LookForEnemy(maxSightRangeSqr);    

            if(newEnemy != null)
            {
                // looking for new enemy when enemy already is assigned, only replace if new enemy is closer than old enemy
                enemy = newEnemy;
            }
            else if(IsEnemyValid() == true && eyes.HasLosToTarget(enemy) == false)
            {
                // clear old enemy if no line of sight
                enemy = null;
            }
        }



        public bool IsEnemyValid()
        {
            if(enemy != null && IsInstanceValid(enemy))
            {
                return true;
            }

            return false;
        }



        public float GetDistanceSqrToEnemy()
        {
            if(IsEnemyValid())
            {
                return GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition);
            }
            else 
            {
                return float.PositiveInfinity;
            }
        }



        public void AllyHurt()
        {
            isAggro = true;
        }



        public void AllySpottedEnemy(MobFaction spottedEnemy)
        {
            enemy = spottedEnemy;
        }



        public void AggroAllies()
        {
            // get allies
            var allies = detection.GetAllies(maxSightRangeForAlliesSqr);

            // alert nearby allies that this mob died
            foreach(MobFaction ally in allies)
            {
                var allyBase = (IMobAlly) ally.Owner;
                allyBase.AllyHurt();
            }
        }



        public void SpotEnemyForAllies()
        {
            // get allies
            var allies = detection.GetAllies(maxSightRangeForAlliesSqr);

            // alert nearby allies that this mob spotted an enemy
            foreach(MobFaction ally in allies)
            {
                var allyBase = (IMobAlly) ally.Owner;
                allyBase.AllySpottedEnemy(enemy);
            }
        }
    }
}