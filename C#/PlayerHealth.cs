using Godot;
using System;

public partial class PlayerHealth : Health
{

    // inherited maxHitPoints is unused
    Disconnector healDisconnector = new Disconnector();



    public override void _Ready()
    {
        // get health values
        hitPoints = PlayerStatistics.statistics.currentStatistics.HitPoints;
        maxHitPoints = PlayerStatistics.statistics.GetMaxHitPoints();
    }



    public override void _Process(double delta)
    {
        // check for heal input
        if(healDisconnector.Trip(PlayerInput.heal))
        {
            // check that hit points are not full and bandages are available
            if(CheckHitPointsNotMaxxed() && PlayerInventory.inventory.CheckInventoryForBandages())
            {
                // apply bandage
                Heal(PlayerStatistics.statistics.currentStatistics.HitPointsPerBandage);

                // remove bandage from inventory
                PlayerInventory.inventory.AddToInventory(0, 0, 0, -1, null);
            }
        }
    }



    public bool CheckHitPointsNotMaxxed()
    {
        return hitPoints < maxHitPoints;
    }



    public override void Heal(float hp)
    {
        // apply healing
        hitPoints = Mathf.Clamp(hitPoints + hp, 0, PlayerStatistics.statistics.GetMaxHitPoints());

        // update statistics
        PlayerStatistics.statistics.UpdateHitPoints(hitPoints);
    }



    public override void Damage(float dmg)
    {
        // apply armor
        var damageAfterArmor = (1 - PlayerStatistics.statistics.GetArmor()) * dmg;

        // apply damage
        hitPoints = Mathf.Clamp(hitPoints - damageAfterArmor, 0, PlayerStatistics.statistics.GetMaxHitPoints());

        // update statistics
        PlayerStatistics.statistics.UpdateHitPoints(hitPoints);
    }
}
