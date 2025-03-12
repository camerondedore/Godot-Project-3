using Godot;
using System;

public partial class TriggerArea : Area3D, IActivatable
{

    [Export]
    Node[] linkedObjects;
    [Export]
    bool saveToWorldData = false,
        monitoringAtStart = true;



    public override void _Ready()
        {
            if(saveToWorldData)
            {
                // get if trigger was already activated
                var wasActivated = WorldData.data.CheckActivatedObjects(this);

                if(wasActivated)
                {
                    // remove cinematic
                    QueueFree();  
                    
                    return;
                }
            }

            // set monitoring
            SetDeferred("monitoring", monitoringAtStart);

            // set up signal
            BodyEntered += Triggered;
        }



    void Triggered(Node3D body)
    {
        ActivateLinkedNodes();

        if(saveToWorldData == true)
        {            
            // save to pickups taken
            WorldData.data.ActivateObject(this);
        }

        QueueFree();
    }



    void ActivateLinkedNodes()
    {
        if(linkedObjects.Length > 0)
        {
            // activate linked objects
            foreach(IActivatable i in linkedObjects)
            {
                if(IsInstanceValid((Node)i) == true)
                {
                    i.Activate();
                }
            }
        }
    }



    public void Activate()
    {
        SetDeferred("monitoring", true);
    }



    public void Deactivate()
    {
        SetDeferred("monitoring", false);
    }
}