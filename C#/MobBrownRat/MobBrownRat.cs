using Godot;
using System;
using PlayerBow;
using CinematicSimple;


namespace MobBrownRat
{
    public partial class MobBrownRat : Mob, MobSpawner.iMobSpawnable
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateStart,
            stateIdle,
            stateReact,
            stateMove,
            stateWatch,
            statePatrol,
            statePatrolWait,
            stateFlee,
            stateAttack,
            stateDraw,
            stateFire,
            stateDodge,
            stateCooldown,
            stateRetreat,
            stateDie;

        [Export]
        public Node3D startTarget;
        [Export]
        public bool isMovingRat = true;

        public NavigationAgent3D navAgent;
        public MobBow bow;
        public MobFaction myFaction1,
            myFaction2;
        public AnimationTree animation;
        public CollisionShape3D collider;
        public AnimationNodeStateMachinePlayback animStateMachinePlayback;
        public Vector3 startPosition;
        public double aimTime = 0.5f,
            fireTime = 1.2f,
            reactTime = 0.4f;
        public float moveRecalculatePathRange = 3,
            attackRangeMinSqr = 225,
            attackRangeMaxSqr = 400,
            fleeRangeSqr = 25,
            fleeMoveDistance = 5,
            fleeSpreadRange = 3,
            PatrolRange = 10,
            lookSpeed = 15f,
            acceleration = 4,
            dodgeDistance = 4,
            lookAngleNormal = 30;
        public int shotCount,
            fleeCount;
        public bool lookAtTarget = false,
            moving;

        bool delay = false,
            isOnNavmesh;
        //string debugText;



        public override void _Ready()
        {
            // change base fields
            speed = 4.5f;

            // get nodes
            detection = (MobDetection) GetNode("Detection");
            eyes = (MobEyes) GetNode("Bow/BowDetection");
            navAgent = (NavigationAgent3D) GetNode("NavAgent");
            bow = (MobBow) GetNode("Bow");
            health = (Health) GetNode("Health");
            myFaction1 = (MobFaction) GetNode("Faction1");
            myFaction2 = (MobFaction) GetNode("Faction2");
            animation = (AnimationTree) GetNode("AnimationTree");
            collider = (CollisionShape3D) GetNode("RatCollider");


            // set nav agent event
            navAgent.VelocityComputed += SafeMove;

            animStateMachinePlayback = (AnimationNodeStateMachinePlayback) animation.Get("parameters/playback");

            // initialize states
            stateStart = new MobBrownRatStateStart(){blackboard = this};
            stateIdle = new MobBrownRatStateIdle(){blackboard = this};
            stateReact = new MobBrownRatStateReact(){blackboard = this};
            stateMove = new MobBrownRatStateMove(){blackboard = this};
            stateWatch = new MobBrownRatStateWatch(){blackboard = this};
            statePatrol = new MobBrownRatStatePatrol(){blackboard = this};
            statePatrolWait = new MobBrownRatStatePatrolWait(){blackboard = this};
            stateFlee = new MobBrownRatStateFlee(){blackboard = this};
            stateAttack = new MobBrownRatStateAttack(){blackboard = this};
            stateDraw = new MobBrownRatStateDraw(){blackboard = this};
            stateFire = new MobBrownRatStateFire(){blackboard = this};
            stateDodge = new MobBrownRatStateDodge(){blackboard = this};
            stateCooldown = new MobBrownRatStateCooldown(){blackboard = this};
            stateRetreat = new MobBrownRatStateRetreat(){blackboard = this};
            stateDie = new MobBrownRatStateDie(){blackboard = this};

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


            isOnNavmesh = NavigationServer3D.MapGetClosestPoint(navAgent.GetNavigationMap(), GlobalPosition).DistanceSquaredTo(GlobalPosition) < 1f;

            // check that rat is in moving state
            if(moving && IsOnFloor() && isOnNavmesh)
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
            else if(moving && IsOnFloor() && isOnNavmesh == false && IsEnemyValid())
            {
                // get new velocity
                var newVelocity = navAgent.TargetPosition - GlobalPosition;
                newVelocity = newVelocity.Normalized();
                newVelocity.Y = 0;
                newVelocity *= speed;
                
                // move
                Velocity = newVelocity;
                MoveAndSlide();

                // get direction to next path point and flatten
                var forward = GlobalPosition + -Basis.Z;
                var lookDirection = GlobalPosition + Velocity.Normalized();
                lookDirection.Y = GlobalPosition.Y;
                var lookTarget = forward.Lerp(lookDirection, lookSpeed * ((float)GetPhysicsProcessDeltaTime()));

                // look in direction of movement
                LookAt(lookTarget, Vector3.Up);
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
                var lookTarget = forward.Lerp(lookDirection, lookSpeed * ((float)delta));

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
                    // clamp safe velocity
                    safeVel = safeVel.LimitLength(speed);
                    
                    // move using obstacle avoidance
                    Velocity = safeVel;
                    MoveAndSlide();

                    // get direction to next path point and flatten
                    var forward = GlobalPosition + -Basis.Z;
                    var lookDirection = GlobalPosition + safeVel.Normalized();
                    lookDirection.Y = GlobalPosition.Y;
                    var lookTarget = forward.Lerp(lookDirection, lookSpeed * ((float)GetPhysicsProcessDeltaTime()));

                    // look in direction of movement
                    LookAt(lookTarget, Vector3.Up);

                    // adjust walk animation speed
                    // var walkSpeed = Velocity.LengthSquared() / Mathf.Pow(speed, 2);
                    // animation.Set("parameters/brown-rat-walk/WalkSpeed/scale", walkSpeed);
                }
            }
        }



        public float GetBowAngleToEnemy()
        {
            if(IsEnemyValid() == false)
            {
                return 0;
            }

            var directionToEnemy = enemy.GlobalPosition - bow.GlobalPosition;
            var distance = new Vector2(directionToEnemy.X, directionToEnemy.Z).Length();
            var height = directionToEnemy.Y;

            var angle = Mathf.RadToDeg(Mathf.Atan(height / distance));

            return angle;
        }



        public void SetTarget(Node3D newTarget)
        {
            startTarget = newTarget;

            machine.SetState(stateStart);
            machine.CurrentState.StartState();
        }
    }
}