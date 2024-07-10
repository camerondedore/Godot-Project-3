using Godot;
using System;
using PlayerBow;

public partial class ClayPotTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string arrowType = "blade";
    [Export]
    PackedScene storedItem;
    [Export]
    Vector3 positionOffset;

    RigidbodySpawner pickupSpawner;



    public override void _Ready()
    {
        // get if cauldron was already activated
        var wasActivated = WorldData.data.CheckActivatedObjects(this);

        if(wasActivated)
        {
            QueueFree();
        }

        pickupSpawner = (RigidbodySpawner) GetNode("PickupSpawner");
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            return ToGlobal(positionOffset);
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit(Vector3 dir)
    {
        // create fx
        //var newFx = cutFx.Instantiate() as Node3D;

        // set transform
        //newFx.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        //GetTree().CurrentScene.AddChild(newFx);
        //newFx.Owner = GetTree().CurrentScene;

        // spawn pickup
        pickupSpawner.Spawn(storedItem);

        // save to activated objects
        WorldData.data.ActivateObject(this);

        // activate gibs


        // destroy
        QueueFree();
    }
}