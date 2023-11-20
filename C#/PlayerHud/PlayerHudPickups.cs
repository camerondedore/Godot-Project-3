using Godot;
using System;
using System.Diagnostics;

public partial class PlayerHudPickups : Control
{

    [Export]
    Control pickupStart,
        candiedNutsPickupEnd,
        dockLeafPickupEnd,
        saniclePickupEnd,
        rangerBandagePickupEnd,
        armorPickupEnd,
        ciderPickupEnd,
        arrowPickupEnd;
    [Export]
    PackedScene candiedNutsPickup,
        dockLeafPickup,
        saniclePickup,
        rangerBandagePickup,
        armorPickup,
        ciderPickup,
        arrowWeightedPickup,
        arrowBladePickup,
        arrowPickPickup,
        arrowNetPickup,
        arrowFirePickup;

    

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



     public void AddArrow(string arrowType)
    {   
        switch(arrowType)
        {
            case "weighted":
                SpawnPickup(arrowWeightedPickup, pickupStart.Position, arrowPickupEnd.Position);
                break;
            case "blade":
                SpawnPickup(arrowBladePickup, pickupStart.Position, arrowPickupEnd.Position);
                break;
            case "pick":
                SpawnPickup(arrowPickPickup, pickupStart.Position, arrowPickupEnd.Position);
                break;
            case "net":
                SpawnPickup(arrowNetPickup, pickupStart.Position, arrowPickupEnd.Position);
                break;
            case "fire":
                SpawnPickup(arrowFirePickup, pickupStart.Position, arrowPickupEnd.Position);
                break;
        }
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