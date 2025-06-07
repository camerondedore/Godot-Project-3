using Godot;
using System;

namespace MobShieldRat;

public partial class MobShieldRat : Mob, MobSpawner.iMobSpawnable
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
            stateShieldBreak,
            stateDie;

    [Export]
    public Node3D startTarget;

    public NavigationAgent3D navAgent;
    public MobFaction myFaction1,
        myFaction2;
    public AnimationPlayer animation;
    public MobShieldRatAudio audio;
    public ParticleFx axeHitFx,
        axeHitBloodFx;
    public CollisionShape3D collider;
    public BoneLookAtControl headControl;
    public GibsActivator gibs;
    public MeshInstance3D shieldMesh;
    public GpuParticles3D shieldBreakFx;

    public Vector3 startPosition,
        dirToNextPathPoint,
        avoidanceDir,
        arrowHitDirection;
    public double swingTime = 0.66,
        attackDamageTime = 0.3,
        reactTime = 0.4,
        shieldBreakTime = 0.66;
    public float moveRecalculatePathRange = 0.5f,
        attackRangeSqr = 3.1f,
        damageRangeSqr = 5.1f,
        attackAngle = 45,
        damage = 10,
        PatrolRange = 10,
        lookSpeed = 15f,
        acceleration = 4;
    public bool lookAtTarget = false,
        moving,
        hasShield = true;

    bool delay = false,
        isOnNavmesh;



    public override void _Ready()
    {            
        // get nodes
        detection = (MobDetection) GetNode("Detection");
        eyes = (MobEyes) GetNode("Eyes");
        navAgent = (NavigationAgent3D) GetNode("NavAgent");
        health = (Health) GetNode("Health");
        myFaction1 = (MobFaction) GetNode("Faction1");
        myFaction2 = (MobFaction) GetNode("Faction2");
        animation = (AnimationPlayer) GetNode("character-shield-rat/AnimationPlayer");
        audio = (MobShieldRatAudio) GetNode("RatAudio");
        axeHitFx = (ParticleFx) GetNode("FxEnemyHit");
        axeHitBloodFx = (ParticleFx) GetNode("FxBloodSlash");
        collider = (CollisionShape3D) GetNode("RatCollider");
        headControl = (BoneLookAtControl) GetNode("character-shield-rat/character-armature/Skeleton3D/HeadLookAt");
        gibs = (GibsActivator) GetNode("character-shield-rat/character-armature/Skeleton3D/ShieldBoneCopy/Gibs");
        shieldMesh = (MeshInstance3D) GetNode("character-shield-rat/character-armature/Skeleton3D/weapon-shield/weapon-shield");
        shieldBreakFx = (GpuParticles3D) GetNode("FxShieldBreak");

        // set nav agent event
        navAgent.VelocityComputed += SafeMove;

        // initialize states
        stateStart = new MobShieldRatStateStart(){blackboard = this};
        superStateIdle = new MobShieldRatSuperStateIdle(){blackboard = this};
        subStateIdle = new MobShieldRatSubStateIdle(){blackboard = this};
        subStateIdleAnimation = new MobShieldRatSubStateIdleAnimation(){blackboard = this};
        stateReact = new MobShieldRatStateReact(){blackboard = this};
        stateMove = new MobShieldRatStateMove(){blackboard = this};
        stateWatch = new MobShieldRatStateWatch(){blackboard = this};
        statePatrol = new MobShieldRatStatePatrol(){blackboard = this};
        statePatrolWait = new MobShieldRatStatePatrolWait(){blackboard = this};
        stateAttack = new MobShieldRatStateAttack(){blackboard = this};
        stateCooldown = new MobShieldRatStateCooldown(){blackboard = this};
        stateRetreat = new MobShieldRatStateRetreat(){blackboard = this};
        stateShieldBreak = new MobShieldRatStateShieldBreak(){blackboard = this};
        stateDie = new MobShieldRatStateDie(){blackboard = this};

        // change mob values
        arrowType = "weighted";

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
        var distanceToEnemySqr = GetDistanceSqrToEnemy();

        var isCloseToEnemy = distanceToEnemySqr < attackRangeSqr;
        var isNotAboveEnemy = (enemy.GlobalPosition.Y - GlobalPosition.Y) > -0.75f;

        // check if enemy is close enough
        if(isCloseToEnemy && isNotAboveEnemy)
        {
            return true;
        }

        return false;
    }



    public bool CanDamageEnemy()
    {
        // get distance to enemy
        var distanceToEnemySqr = GetDistanceSqrToEnemy();
        var angleForwardToEnemy = GetForwardToEnemyAngle();
        var angleUpToEnemy = GetUpAngleToEnemy();

        var enemyInFront = distanceToEnemySqr < damageRangeSqr && angleForwardToEnemy < attackAngle;
        var enemyAbove = distanceToEnemySqr < damageRangeSqr * 2f && angleUpToEnemy < attackAngle;
        var enemyClose = distanceToEnemySqr < 0.3f;

        if(enemyInFront || enemyAbove || enemyClose)
        {
            return true;
        }

        return false;
    }



    public override bool Hit(Vector3 dir)
    {
        if(hasShield == true)
        {
            // break shield
            machine.SetState(stateShieldBreak);
            arrowHitDirection = dir;
        }
        else
        {
            // take damage from arrow
            health.Damage(damageFromArrow);
        }

        return true;
    }



    public bool IsAvoidanceDirectionFarFromPath()
    {
        var angle = Mathf.RadToDeg(avoidanceDir.AngleTo(dirToNextPathPoint));

        return angle > 50;
    }
}