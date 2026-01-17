using Godot;
using System;
using PlayerBow;
using CinematicSimple;


namespace MobBlackRat;

public partial class MobBlackRat : Mob, MobSpawner.iMobSpawnable
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


    
    public NavigationAgent3D navAgent;
    public MobFaction myFaction1,
        myFaction2;
    public AnimationPlayer animation;
    public MobBlackRatAudio audio;
    public ParticleFx swordHitFx,
        swordHitBloodFx;
    public CollisionShape3D collider;
    public BoneLookAtControl headControl;
    
    public Vector3 startPosition,
        dirToNextPathPoint,
        avoidanceDir;
    public double swingTime = 0.5,
        attackDamageTime = 0.3,
        reactTime = 0.4,
        idleAnimationTime = 4.0;
    public float moveRecalculatePathRange = 0.5f,
        attackRange = 1.75f,
        attackRangeUp = 2.25f,
        damageRange = 2.25f,
        damageRangeUp = 2.75f,
        attackAngle = 45,
        damage = 10,
        PatrolRange = 10,
        lookSpeed = 15f,
        acceleration = 4;
    public bool lookAtTarget = false,
        moving;

    bool delay = false,
        isOnNavmesh;
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
        headControl = (BoneLookAtControl) GetNode("character-black-rat/character-armature/Skeleton3D/HeadLookAt");

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
        if(moving && IsOnFloor() && IsOnNavmesh(navAgent) == true)
        {
            // get new velocity
            var newVelocity = navAgent.GetNextPathPosition() - GlobalPosition;
            newVelocity = newVelocity.Normalized();
            dirToNextPathPoint = newVelocity;
            newVelocity.Y = 0;
            newVelocity *= speed;

            // smooth movement
            newVelocity = Velocity.Lerp(newVelocity, acceleration * ((float) delta));
            
            // pass new velocity to nav agent
            navAgent.Velocity = newVelocity;
        }
        else if(moving && IsOnFloor() && IsOnNavmesh(navAgent) == false && IsEnemyValid())
        {
            // get new velocity
            var newVelocity = enemy.GlobalPosition - GlobalPosition;
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

            // adjust walk animation speed
            var walkSpeed = Velocity.LengthSquared() / Mathf.Pow(speed, 2);
            //animation.Set("parameters/brown-rat-walk/WalkSpeed/scale", walkSpeed);
            animation.SpeedScale = walkSpeed;
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
                // clamp safe velocity
                safeVel = safeVel.LimitLength(speed);

                avoidanceDir = safeVel.Normalized();

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
        }
    }       



    public void SetTarget(Node3D newTarget)
    {
        startTarget = newTarget;

        machine.SetState(stateStart);
        machine.CurrentState.StartState();
    }



    public bool CanAttackEnemy()
    {
        var distanceToEnemy = GetDistanceToEnemy();

        var isCloseToEnemy = distanceToEnemy < attackRange;
        var isNotAboveEnemy = (enemy.GlobalPosition.Y - GlobalPosition.Y) > -0.75f;
        
        var isBelowEnemy = distanceToEnemy < attackRangeUp && GetUpAngleToEnemy() < attackAngle;

        // check if enemy is close enough or if enemy is above
        if((isCloseToEnemy && isNotAboveEnemy) || isBelowEnemy)
        {
            return true;
        }

        return false;
    }



    public bool CanDamageEnemy()
    {
        // get distance to enemy
        var distanceToEnemy = GetDistanceToEnemy();
        var angleForwardToEnemy = GetForwardToEnemyAngle();
        var angleUpToEnemy = GetUpAngleToEnemy();

        var enemyInFront = distanceToEnemy < damageRange && angleForwardToEnemy < attackAngle;
        var enemyAbove = distanceToEnemy < damageRangeUp && angleUpToEnemy < attackAngle;
        var enemyClose = distanceToEnemy < 0.8f;

        if(enemyInFront || enemyAbove || enemyClose)
        {
            return true;
        }

        return false;
    }



    public bool IsAvoidanceDirectionFarFromPath()
    {
        var angle = Mathf.RadToDeg(avoidanceDir.AngleTo(dirToNextPathPoint));

        return angle > 50;
    }
}