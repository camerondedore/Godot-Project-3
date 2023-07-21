using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MobDetection : RayCast3D
{

    [Export]
    public MobFaction myFaction;

    [Export]
    public float maxRangeSqr = 100;



    public override void _Ready()
    {
        Enabled = false;
    }



    public MobFaction LookForEnemy(float maxRangeSqr)
    {
        // get list of enemies
        var enemies = MobFaction.mobs.Where(m => m.faction != myFaction.faction).ToArray();

        // order list of enemies by distance
        var enemiesOrdered = enemies.Where(m => m.GlobalPosition.DistanceSquaredTo(this.GlobalPosition) < maxRangeSqr).OrderBy(m => m.GlobalPosition.DistanceSquaredTo(this.GlobalPosition)).ToArray();

        // use ray to check LOS to enemies
        foreach(var enemy in enemiesOrdered)
        {
            // set ray to look at enemy
            TargetPosition = this.ToLocal(enemy.GlobalPosition);

            // cast ray
            ForceRaycastUpdate();

            // check hit node is enemy
            if(GetCollider() != null)
            {
                var hitCollider = (Node3D) GetCollider();

                var rayHitEnemy = hitCollider == enemy.Owner || hitCollider.Owner == enemy.Owner;

                if(rayHitEnemy)
                {
                    // enemy found
                    return enemy;
                }
            }
        }

        // no enemy found or no enemy visable
        return null;
    }



    public List<MobFaction> GetAllies(float maxRangeSqr)
    {
        // get list of allies within range
        var allies = MobFaction.mobs.Where(m => m.faction == myFaction.faction).Where(m => m.GlobalPosition.DistanceSquaredTo(this.GlobalPosition) < maxRangeSqr).ToList();

        return allies;
    }
}
