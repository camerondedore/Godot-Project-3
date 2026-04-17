using Godot;
using System;
using System.Diagnostics;

namespace MobChampionRat;

public partial class MobChampionRat : Mob, MobSpawner.iMobSpawnable
{

    public StateMachineQueue machine = new StateMachineQueue();
    //public SuperState superStateIdle;
    public State stateStart,
        stateIdle,
        stateFall,
        stateReact,
        stateMove,
        stateAttack,
        stateHurt,
        stateDeflect,
        stateCooldown,
        stateRetreat,
        stateWatch,
        statePatrol,
        statePatrolWait,
        stateDie;

    [Export]
    public Node3D startTarget;

    public NavigationAgent3D navAgent;
    public MobFaction myFaction1,
        myFaction2;
    public AnimationPlayer animation;
    public MobChampionRatAudio audio;
    public ParticleFx poleaxeHitFx,
        poleaxeHitBloodFx;
    public CollisionShape3D collider;
    public BoneLookAtControl headControl;

    public Vector3 startPosition,
        dirToNextPathPoint,
        avoidanceDir,
        arrowHitDirection;
    public double swingTime = 1.833,
        attackDamageTime = 0.44,
        attackLookStopTime = 0.3,
        attackVulnerableTime = 0.5,
        reactTime = 0.56,
        idleAnimationTime = 3.33,
        hurtAnimationTime = 1.1,
        deflectAnimationTime = 0.6;
    public float moveRecalculatePathRange = 0.5f,
        attackRange = 2.75f,
        attackRangeUp = 3.5f,
        damageRange = 4f,
        damageRangeUp = 3.65f,
        attackAngle = 30,
        damage = 30,
        PatrolRange = 10,
        lookSpeed = 15f,
        acceleration = 6;
    public bool lookAtTarget = false,
        moving,
        vulnerable = false;

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
        animation = (AnimationPlayer) GetNode("character-champion-rat/AnimationPlayer");
        audio = (MobChampionRatAudio) GetNode("RatAudio");
        poleaxeHitFx = (ParticleFx) GetNode("FxEnemyHit");
        poleaxeHitBloodFx = (ParticleFx) GetNode("FxBloodSlash");
        collider = (CollisionShape3D) GetNode("RatCollider");
        headControl = (BoneLookAtControl) GetNode("character-champion-rat/character-armature/Skeleton3D/HeadLookAt");

        // set nav agent event
        navAgent.VelocityComputed += SafeMove;

        // initialize states
        stateStart = new MobChampionRatStateStart(){blackboard = this};
        stateIdle = new MobChampionRatStateIdle(){blackboard = this};
        stateFall = new MobChampionRatStateFall(){blackboard = this};
        stateReact = new MobChampionRatStateReact(){blackboard = this};
        stateMove = new MobChampionRatStateMove(){blackboard = this};
        stateAttack = new MobChampionRatStateAttack(){blackboard = this};
        stateHurt = new MobChampionRatStateHurt(){blackboard = this};
        stateDeflect = new MobChampionRatStateDeflect(){blackboard = this};
        stateCooldown = new MobChampionRatStateCooldown(){blackboard = this};
        stateRetreat = new MobChampionRatStateRetreat(){blackboard = this};
        stateWatch = new MobChampionRatStateWatch(){blackboard = this};
        statePatrol = new MobChampionRatStatePatrol(){blackboard = this};
        statePatrolWait = new MobChampionRatStatePatrolWait(){blackboard = this};
        //stateDie = new MobChampionRatStateDie(){blackboard = this};

        // change mob values
        arrowType = "bodkin";

        // add animation blends
        animation.SetBlendTime("champion-rat-run", "champion-rat-deflect", 0.0);
        animation.SetBlendTime("champion-rat-idle", "champion-rat-deflect", 0.0);
        animation.SetBlendTime("champion-rat-idle-aggro", "champion-rat-deflect", 0.0);
        animation.SetBlendTime("champion-rat-run", "champion-rat-hurt", 0.0);
        animation.SetBlendTime("champion-rat-idle", "champion-rat-hurt", 0.0);
        animation.SetBlendTime("champion-rat-idle-aggro", "champion-rat-hurt", 0.0);
        animation.SetBlendTime("champion-rat-attack", "champion-rat-hurt", 0.0);

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

            // get direction to look in
            var forward = GlobalPosition + -Basis.Z;
            var lookDirection = GlobalPosition + Velocity.Normalized();
            lookDirection.Y = GlobalPosition.Y;
            var lookTarget = forward.Lerp(lookDirection, lookSpeed * ((float)delta));

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

            // get direction to look in
            var forward = GlobalPosition + -Basis.Z;
            var lookDirection = GlobalPosition + Velocity.Normalized();
            lookDirection.Y = GlobalPosition.Y;
            var lookTarget = forward.Lerp(lookDirection, lookSpeed * ((float)delta));

            // look in direction of movement
            LookAt(lookTarget, Vector3.Up);
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
                safeVel.Y = 0;
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



    public override bool Hit(Vector3 dir)
    {
        // check if rat is in vulnerable state or if hitting rat's back

        arrowHitDirection = dir;

        if(vulnerable == true)
        {
            // take damage from arrow
            health.Damage(damageFromArrow);
            machine.SetState(stateHurt);
            return true;
        }
        else
        {
            // deflect arrow
            machine.SetState(stateDeflect);
            return false;
        }

    }
}