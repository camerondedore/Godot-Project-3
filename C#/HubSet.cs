using Godot;
using System;

public partial class HubSet : Node
{

    [Export]
    public int hubSet;
    [Export]
    public Node[] setNodes;



    public void ActivateSet()
    {
        // activate set nodes
        foreach(var setNode in setNodes)
        {
            if(IsInstanceValid(setNode) == false || setNode == null)
            {
                continue;
            }

            // check for lights
            if(setNode is Light3D lightNode)
            {
                lightNode.Visible = true;
            }
            else
            {
                if(setNode is IActivatable setNodeActivatable)
                {
                    // activate node if it can be
                    setNodeActivatable.Activate();
                }
                else
                {
                    setNode.ProcessMode = ProcessModeEnum.Inherit;
                    ((Node3D)setNode).Visible = true;
                }
            }
        }
    }



    public void DeactivateSet()
    {
        // deactivate set nodes
        foreach(var setNode in setNodes)
        {
            if(IsInstanceValid(setNode) == false || setNode == null)
            {
                continue;
            }

            // check for torches
            if(setNode is Torch torchNode)
            {
                torchNode.lit = false;
            }
            else if(setNode is IActivatable setNodeActivatable)
            {
                // deactivate node if it can be
                setNodeActivatable.Deactivate();
            }
            else
            {
                setNode.ProcessMode = ProcessModeEnum.Disabled;
                ((Node3D)setNode).Visible = false;
            }
        }
    }
}
