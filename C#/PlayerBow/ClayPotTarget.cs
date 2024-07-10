using Godot;
using System;
using PlayerBow;

public partial class ClayPotTarget : StaticBody3D, IBowTarget
{

    [Export]
    PackedScene storedItem;

    public string arrowType = "weighted";
    
    GibsActivator gibs;
    RigidbodySpawner pickupSpawner;
    Vector3 positionOffset = new Vector3(0, 0.6f, 0);



    public override void _Ready()
    {
        // get if cauldron was already activated
        var wasActivated = WorldData.data.CheckActivatedObjects(this);

        if(wasActivated)
        {
            QueueFree();
        }
        
        // get nodes
        gibs = (GibsActivator) GetNode("Gibs");
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
        gibs.Activate();

        // destroy
        QueueFree();
    }
}