using Godot;
using System;
using System.Linq;

public partial class MobDetection : RayCast3D
{

    [Export]
    Node3D myFactionNode;

    [Export]
    public float maxRangeSqr = 100;
    
    MobFaction myFaction;


    public override void _Ready()
    {
        Enabled = false;

        myFaction = (MobFaction) myFactionNode;
    }



    public MobFaction LookForEnemy()
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
}
