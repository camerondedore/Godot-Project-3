using Godot;
using System;
using PlayerBow;
using CinematicSimple;


namespace MobBlackRat
{
    public partial class MobBlackRat : CharacterBody3D, IBowTarget, IMobAlly, CinematicSimpleTriggerDead.IWatchable, MobSpawner.iMobSpawnable
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public SuperState superStateIdle;
        public State stateStart,
            subStateIdle,
            subStateIdleAnimation,
            stateReact,
            stateMove,
            stateWatch,
            statePatrol,
            statePatrolWait,
            stateAttack,
            stateCooldown,
            stateRetreat,
            stateDie;

        [Export]
        public Node3D startTarget;


        string arrowType = "bodkin";
        public MobDetection detection;
        public MobEyes eyes;
        public NavigationAgent3D navAgent;
        public Health health;
        public MobFaction myFaction1,
            myFaction2;
        public AnimationPlayer animation;
        public MobBlackRatAudio audio;
        public ParticleFx swordHitFx,
            swordHitBloodFx;
        public CollisionShape3D collider;
        public MobFaction enemy;
        public Vector3 startPosition;
        public double swingTime = .5f,
            attackDamageTime = 0.3f,
            reactTime = 0.4f;
        public float maxSightRangeSqr = 625,
            maxSightRangeForAlliesSqr = 100,
            moveRecalculatePathRange = 0.5f,
            attackDistanceSqr = 4.5f,
            attackRangeSqr = 6.25f,
            attackAngle = 60,
            PatrolRange = 10,
            speed = 5.5f,
            lookSpeed = 15f,
            acceleration = 4,
            damageFromArrow = 50,
            damage = 10;
        public bool lookAtTarget = false,
            moving,
            isAggro;

        bool delay = false;
        //string debugText;



        public override void _Ready()
        {            
            // get nodes
            detection = (MobDetection) GetNode("Detection");
            eyes = (MobEyes) GetNode("Eyes");
            navAgent = (NavigationAgent3D) GetNode("NavAgent");
            health = (Health) GetNode("Health");
            myFaction1 = (MobFaction) GetNode("Faction1");
            myFaction2 = (MobFaction) GetNode("Faction2");
            animation = (AnimationPlayer) GetNode("character-black-rat/AnimationPlayer");
            audio = (MobBlackRatAudio) GetNode("RatAudio");
            swordHitFx = (ParticleFx) GetNode("FxEnemyHit");
            swordHitBloodFx = (ParticleFx) GetNode("FxBloodSlash");
            collider = (CollisionShape3D) GetNode("RatCollider");

            // set nav agent event
            navAgent.VelocityComputed += SafeMove;

            // initialize states
            stateStart = new MobBlackRatStateStart(){blackboard = this};
            superStateIdle = new MobBlackRatSuperStateIdle(){blackboard = this};
            subStateIdle = new MobBlackRatSubStateIdle(){blackboard = this};
            subStateIdleAnimation = new MobBlackRatSubStateIdleAnimation(){blackboard = this};
            stateReact = new MobBlackRatStateReact(){blackboard = this};
            stateMove = new MobBlackRatStateMove(){blackboard = this};
            stateWatch = new MobBlackRatStateWatch(){blackboard = this};
            statePatrol = new MobBlackRatStatePatrol(){blackboard = this};
            statePatrolWait = new MobBlackRatStatePatrolWait(){blackboard = this};
            stateAttack = new MobBlackRatStateAttack(){blackboard = this};
            stateCooldown = new MobBlackRatStateCooldown(){blackboard = this};
            stateRetreat = new MobBlackRatStateRetreat(){blackboard = this};
            stateDie = new MobBlackRatStateDie(){blackboard = this};
        

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

                // reset animation play speed
                animation.SpeedScale = 1;
            }
            else
            {                
                // falling
                // apply gravity
                Velocity += EngineGravity.vector * ((float) delta);                
                MoveAndSlide();

                // reset animation play speed
                animation.SpeedScale = 1;
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
                    var walkSpeed = Velocity.LengthSquared() / Mathf.Pow(speed, 2);
                    //animation.Set("parameters/brown-rat-walk/WalkSpeed/scale", walkSpeed);
                    animation.SpeedScale = walkSpeed;
                }
                // else
                // {
                //     animation.SpeedScale = 1;
                // }
            }
            // else
            // {
            //     animation.SpeedScale = 1;
            // }
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



        public float GetForwardToEnemyAngle()
        {
            if(IsEnemyValid() == false)
            {
                return 0;
            }

            var unitDirectionToEnemy = (enemy.GlobalPosition - eyes.GlobalPosition).Normalized();

            var angle = (-Basis.Z).AngleTo(unitDirectionToEnemy);
        
            return Mathf.RadToDeg(angle);
        }



        public float GetUpToEnemyAngle()
        {
            if(IsEnemyValid() == false)
            {
                return 0;
            }

            var unitDirectionToEnemy = (enemy.GlobalPosition - eyes.GlobalPosition).Normalized();

            var angle = Basis.Y.AngleTo(unitDirectionToEnemy);
        
            return Mathf.RadToDeg(angle);
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



        public bool IsAlive()
        {
            return health.hitPoints > 0;
        }



        public void SetTarget(Node3D newTarget)
        {
            startTarget = newTarget;

            machine.SetState(stateStart);
            machine.CurrentState.StartState();
        }
    }
}