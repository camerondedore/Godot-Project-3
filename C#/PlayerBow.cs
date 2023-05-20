using Godot;
using System;

public partial class PlayerBow : Node3D
{

    [Export]
    PackedScene weightedArrow;



    public void Fire(IBowTarget target)
    {
        // check for target
        if(target == null)
        {
            return;
        }

        var arrowToFire = weightedArrow;

        // NEED TO ADD OTHER ARROWS
        switch(target.GetArrowType())
        {
            case "weighted":
                arrowToFire = weightedArrow;
                break;
            case "pick":
                arrowToFire = weightedArrow;
                break;
        }


        // create new arrow
        var newArrow = (PlayerArrow) weightedArrow.Instantiate();
        // set new arrow position and look direction
        newArrow.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);
        // set target for arrow
        newArrow.target = target;
        // assign to scene
        GetTree().Root.AddChild(newArrow);
        newArrow.Owner = GetTree().Root;
    }
}
