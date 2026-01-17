using Godot;
using System;
using PlayerBow;
using CinematicSimple;

public partial class Mob : CharacterBody3D, IBowTarget, IWatchable, IActivatable
{

    public MobDetection detection;
    public MobEyes eyes;
    public Health health;
    public MobFaction enemy;
    public float damageFromArrow = 50,
        maxSightRangeForAlliesSqr = 100,
        maxSightRangeSqr = 625,
        speed = 5.5f,
        characterHeight = 2f;
    public string arrowType = "bodkin";
    public bool isAggro;



    public virtual void LookForEnemy()
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



    public float GetDistanceToEnemy()
    {
        if(IsEnemyValid())
        {
            return GlobalPosition.DistanceTo(enemy.GlobalPosition);
        }
        else 
        {
            return float.PositiveInfinity;
        }
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



    public float GetUpAngleToEnemy()
    {
        if(IsEnemyValid() == false)
        {
            return 0;
        }

        var unitDirectionToEnemy = (enemy.GlobalPosition - eyes.GlobalPosition).Normalized();

        var angle = Basis.Y.AngleTo(unitDirectionToEnemy);
    
        return Mathf.RadToDeg(angle);
    }



    public void SpotEnemyForAllies()
    {
        // get allies
        var allies = detection.GetAllies(maxSightRangeForAlliesSqr);

        // alert nearby allies that this mob spotted an enemy
        foreach(MobFaction ally in allies)
        {
            var allyBase = (Mob) ally.Owner;
            allyBase.AllySpottedEnemy(enemy);
        }
    }



    public void ClayPotCheck()
    {
        // check for collision with clay pot and break any in the way

        if(GetLastSlideCollision() != null)
        {
            // check for clay pots to break
            var lastHitCollider = GetLastSlideCollision().GetCollider();

            if(lastHitCollider != null && lastHitCollider is ClayPotTarget hitClayPot)
            {
                // break pot
                hitClayPot.Hit(Velocity);
            }
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
            var allyBase = (Mob) ally.Owner;
            allyBase.AllyHurt();
        }
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetTargetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            return GlobalPosition;
        }

        return Vector3.Zero;
    }



    public virtual bool Hit(Vector3 dir)
    {
        // take damage from arrow
        health.Damage(damageFromArrow);
        
        return true;
    }



    public bool IsAlive()
    {
        return health.hitPoints > 0;
    }



    public bool IsOnNavmesh(NavigationAgent3D navAgent)
    {
        var positionForNavCheck = GlobalPosition + Vector3.Down * (characterHeight * 0.5f - 0.25f);
        return NavigationServer3D.MapGetClosestPoint(navAgent.GetNavigationMap(), positionForNavCheck).DistanceSquaredTo(positionForNavCheck) < 0.3f;
    }



    public float GetMaxStuckSpeedSqr()
    {
        return Mathf.Pow(speed * 0.8f, 2f);
    }



    public void Activate()
    {
        // aggro mob
        AllyHurt();
    }



    public void Deactivate()
    {
        isAggro = false;
    }
}