using Godot;
using System;
using PlayerBow;


namespace MobBrownRat
{
    public partial class MobBrownRat : CharacterBody3D, IBowTarget, IMobAlly
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateIdle,
            stateMove,
            statePatrol,
            statePatrolWait,
            stateFlee,
            stateAim,
            stateFire,
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
        public MobBow bow;
        [Export]
        public Health health;
        [Export]
        public string arrowType = "bodkin";
        [Export]
        public double aimTime = 1f,
            fireTime = 0.5f;
        [Export]
        public float maxSightRangeSqr = 100,
            maxSightRangeForAlliesSqr = 100,
            moveRecalculatePathRange = 3,
            attackRangeMinSqr = 225,
            attackRangeMaxSqr = 400,
            fleeRangeSqr = 25,
            fleeMoveDistance = 8,
            fleeSpreadRange = 3,
            PatrolRange = 10,
            speed = 3,
            lookSpeed = 4,
            acceleration = 4,
            dodgeDistance = 2,
            damageFromArrow = 50;
        [Export]
        bool defensive = false;

        public MobFaction enemy;
        public Vector3 startPosition;
        public int shotCount,
            fleeCount;
        public bool lookAtTarget = false,
            allyDied;

        bool delay = false;


        public override void _Ready()
        {            
            startPosition = GlobalPosition;

            // set nav agent event
            navAgent.VelocityComputed += SafeMove;

            // initialize states
            stateIdle = new MobBrownRatStateIdle(){blackboard = this};
            stateMove = new MobBrownRatStateMove(){blackboard = this};
            statePatrol = new MobBrownRatStatePatrol(){blackboard = this};
            statePatrolWait = new MobBrownRatStatePatrolWait(){blackboard = this};
            stateFlee = new MobBrownRatStateFlee(){blackboard = this};
            stateAim = new MobBrownRatStateAim(){blackboard = this};
            stateFire = new MobBrownRatStateFire(){blackboard = this};
            stateDodge = new MobBrownRatStateDodge(){blackboard = this};
            stateCooldown = new MobBrownRatStateCooldown(){blackboard = this};
            stateRetreat = new MobBrownRatStateRetreat(){blackboard = this};
            stateDie = new MobBrownRatStateDie(){blackboard = this};

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
            // one-frame delay
            if(delay == false)
            {
                delay = true;
                return;
            }


            // check that target isn't current position
            if(navAgent.TargetPosition != GlobalPosition)
            {
                var newVelocity = Velocity;

                if(IsOnFloor())
                {
                    // get new velocity
                    newVelocity = navAgent.GetNextPathPosition() - GlobalPosition;
                    newVelocity = newVelocity.Normalized();
                    newVelocity *= speed;
                }
                else
                {
                    // apply gravity
                    newVelocity += EngineGravity.vector * ((float) delta);
                }

                // smooth movement
                newVelocity = Velocity.Lerp(newVelocity, acceleration * ((float) delta));
                
                // pass new velocity to nav agent
                navAgent.Velocity = newVelocity;
            }


            if(enemy != null && lookAtTarget)
            {
                try
                {
                    // get direction to enenmy and flatten
                    var lookTarget = enemy.GlobalPosition;
                    lookTarget.Y = GlobalPosition.Y;

                    // look at enemy
                    LookAt(lookTarget);
                }
                catch
                {
                    // enemy is disposed
                }
            }
            else if(Velocity.LengthSquared() > 0)
            {
                // get direction to next path point and flatten
                var lookTarget = GlobalPosition + Velocity;
                lookTarget.Y = GlobalPosition.Y;

                // look in direction of movement
                LookAt(lookTarget);
            }
        }



        void SafeMove(Vector3 safeVel)
        {
            Velocity = safeVel;
            MoveAndSlide();
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
            // take damage from arrow
            health.Damage(damageFromArrow);
        }



        public void AllyKilled()
        {
            allyDied = true;
        }
    }
}