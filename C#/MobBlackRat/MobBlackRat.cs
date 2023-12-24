using Godot;
using System;
using PlayerBow;


namespace MobBlackRat
{
    public partial class MobBlackRat : CharacterBody3D, IBowTarget, IMobAlly
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateStart,
            stateIdle,
            stateReact,
            stateMove,
            stateWatch,
            statePatrol,
            statePatrolWait,
            stateAttack,
            stateDodge,
            stateCooldown,
            stateRetreat,
            stateDie;

        [Export]
        public MobDetection detection;
        [Export]
        public MobEyes eyes;
        [Export]
        public NavigationAgent3D navAgent;
        [Export]
        public Health health;
        [Export]
        public MobFaction[] myFactions;
        [Export]
        public AnimationTree animation;
        [Export]
        public CollisionShape3D collider;
        [Export]
        public Node3D startTarget;
        [Export]
        public string arrowType = "bodkin";
        [Export]
        public double attackTime = 1f,
            reactTime = 0.4f;
        [Export]
        public float maxSightRangeSqr = 100,
            maxSightRangeForAlliesSqr = 100,
            moveRecalculatePathRange = 3,
            attackRangeSqr = 1,
            PatrolRange = 10,
            speed = 3,
            lookSpeed = 4,
            acceleration = 4,
            damageFromArrow = 50,
            lookAngleNormal = 30;

        public MobFaction enemy;
        public AnimationNodeStateMachinePlayback animStateMachinePlayback;
        public Vector3 startPosition;
        public bool lookAtTarget = false,
            moving,
            isAggro;

        //string debugText;
        bool delay = false;



        public override void _Ready()
        {            
            // set nav agent event
            navAgent.VelocityComputed += SafeMove;

            animStateMachinePlayback = (AnimationNodeStateMachinePlayback) animation.Get("parameters/playback");

            // initialize states
            stateStart = new MobBlackRatStateStart(){blackboard = this};
            stateIdle = new MobBlackRatStateIdle(){blackboard = this};
        

            // set first state in machine
            machine.SetState(stateStart);
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
            // debug
			// if(debugText != machine.CurrentState.ToString())
			// {
			// 	GD.Print(machine.CurrentState.ToString());
			// 	debugText = machine.CurrentState.ToString();
			// }   

            // one-frame delay
            if(delay == false)
            {
                delay = true;
                return;
            }


            // check that rat is in moving state
            if(moving && IsOnFloor())
            {
                // get new velocity
                var newVelocity = navAgent.GetNextPathPosition() - GlobalPosition;
                newVelocity = newVelocity.Normalized();
                newVelocity.Y = 0;
                newVelocity *= speed;

                // smooth movement
                newVelocity = Velocity.Lerp(newVelocity, acceleration * ((float) delta));
                
                // pass new velocity to nav agent
                navAgent.Velocity = newVelocity;
            }
            else if(IsOnFloor())
            {
                // pass zero velocity to nav agent
                Velocity = Vector3.Zero;
                MoveAndSlide();
            }
            else
            {                
                // falling
                // apply gravity
                Velocity += EngineGravity.vector * ((float) delta);                
                MoveAndSlide();
            }


            // check if rat is looking at target
            if(lookAtTarget && !moving && IsInstanceValid(enemy))
            {
                // get direction to enenmy and flatten
                var forward = GlobalPosition + -Basis.Z;
                var lookDirection = GlobalPosition + (enemy.GlobalPosition - GlobalPosition).Normalized();
                lookDirection.Y = GlobalPosition.Y;
                var lookTarget = forward.Lerp(lookDirection, lookSpeed);

                // look at enemy
                LookAt(lookTarget);
            }
        }



        void SafeMove(Vector3 safeVel)
        {
            if(moving && IsOnFloor())
            {        
                // check that velocity is not zero
                if(safeVel.X != 0 || safeVel.Z != 0)
                {
                    // move using obstacle avoidance
                    Velocity = safeVel;
                    MoveAndSlide();

                    // get direction to next path point and flatten
                    var forward = GlobalPosition + -Basis.Z;
                    var lookDirection = GlobalPosition + safeVel.Normalized();
                    lookDirection.Y = GlobalPosition.Y;
                    var lookTarget = forward.Lerp(lookDirection, lookSpeed);

                    // look in direction of movement
                    LookAt(lookTarget, Vector3.Up);

                    // adjust walk animation speed
                    var walkSpeed = Velocity.LengthSquared() / Mathf.Pow(speed, 2);
                    animation.Set("parameters/brown-rat-walk/WalkSpeed/scale", walkSpeed);
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
            // take damage from arrow
            health.Damage(damageFromArrow);
        }



        public void LookForEnemy()
        {
            // look for new enemy
            // var lookDistanceSqr = IsEnemyValid() ? GetDistanceSqrToEnemy() : maxSightRangeSqr;

            // var newEnemy = detection.LookForEnemy(lookDistanceSqr);
            var newEnemy = detection.LookForEnemy(maxSightRangeSqr);

            if(newEnemy != null)
            {
                // looking for new enemy when enemy already is assigned, only replace if new enemy is closer than old enemy
                enemy = newEnemy;
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



        public float GetLookAngleToEnemy()
        {
            if(IsEnemyValid() == false)
            {
                return 0;
            }

            var directionToEnemy = enemy.GlobalPosition - eyes.GlobalPosition;
            var distance = new Vector2(directionToEnemy.X, directionToEnemy.Z).Length();
            var height = directionToEnemy.Y;

            var unsignedAngle = Mathf.RadToDeg(Mathf.Atan(height / distance));

            //GD.Print(distance + ", " + height + ", " + "angle " + unsignedAngle);

            return unsignedAngle;
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