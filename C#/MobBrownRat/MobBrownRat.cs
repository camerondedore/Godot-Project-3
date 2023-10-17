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
            stateAttack,
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
        public AnimationTree animation;
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

        public MobFaction enemy;
        public Vector3 startPosition;
        public int shotCount,
            fleeCount;
        public bool lookAtTarget = false,
            moving,
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
            stateAttack = new MobBrownRatStateAttack(){blackboard = this};
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
            if(lookAtTarget && !moving && enemy != null)
            {
                try
                {
                    // get direction to enenmy and flatten
                    var forward = GlobalPosition + -Basis.Z;
                    var lookDirection = GlobalPosition + (enemy.GlobalPosition - GlobalPosition).Normalized();
                    lookDirection.Y = GlobalPosition.Y;
                    var lookTarget = forward.Lerp(lookDirection, lookSpeed);

                    // look at enemy
                    LookAt(lookTarget);
                }
                catch
                {
                    // enemy is disposed
                }
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
                }
            }
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