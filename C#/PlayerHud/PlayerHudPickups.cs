using Godot;
using System;

public partial class PlayerHudPickups : Control
{

    [Export]
    Control pickupStart,
        candiedNutsPickupEnd,
        dockLeafPickupEnd,
        saniclePickupEnd,
        rangerBandagePickupEnd,
        armorPickupEnd,
        ciderPickupEnd;
    [Export]
    PackedScene candiedNutsPickup,
        dockLeafPickup,
        saniclePickup,
        rangerBandagePickup,
        armorPickup,
        ciderPickup;

    

    public void AddCandiedNut()
    {   
        SpawnPickup(candiedNutsPickup, pickupStart.Position, candiedNutsPickupEnd.Position);
    }



    public void AddDockLeaf()
    {   
        SpawnPickup(dockLeafPickup, pickupStart.Position, dockLeafPickupEnd.Position);
    }



    public void RemoveDockLeaf()
    {   
        SpawnPickup(dockLeafPickup, dockLeafPickupEnd.Position, pickupStart.Position);
    }



    public void AddSanicle()
    {   
        SpawnPickup(saniclePickup, pickupStart.Position, saniclePickupEnd.Position);
    }



    public void RemoveSanicle()
    {   
        SpawnPickup(saniclePickup, saniclePickupEnd.Position, pickupStart.Position);
    }



    public void AddRangerBandage()
    {   
        SpawnPickup(rangerBandagePickup, pickupStart.Position, rangerBandagePickupEnd.Position);
    }



    public void AddArmor()
    {   
        SpawnPickup(armorPickup, pickupStart.Position, armorPickupEnd.Position);
    }



    public void AddHitpointsBar()
    {   
        SpawnPickup(ciderPickup, pickupStart.Position, ciderPickupEnd.Position);
    }



    void SpawnPickup(PackedScene prefab, Vector2 startPosition, Vector2 endPosition)
    {
        // create pickup
        var newPickup = (PlayerHudPickup) prefab.Instantiate();

        // set new pickup transfrom
        newPickup.Position = startPosition;

        // set new pickup parent and owner
        AddChild(newPickup);
        newPickup.Owner = this;

        // set fields for lerp
        newPickup.endPosition = endPosition;
    }
}