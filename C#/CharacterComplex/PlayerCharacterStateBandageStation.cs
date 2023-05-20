using Godot;
using System;

public partial class PlayerCharacterStateBandageStation : PlayerCharacterState
{

    double startTime;



    public override void RunState(double delta)
    {
        // check for bandage components
        var hasComponents = PlayerInventory.inventory.CheckInventoryForBandageComponents();

        if(hasComponents && EngineTime.timePassed > startTime + blackboard.bandageTime)
        {
            // create bandage
            PlayerInventory.inventory.AddToInventory(0, -1, -1, 1, null);

            // reset timer
            startTime = EngineTime.timePassed;  
        }
    }



    public override void StartState()
    {
        startTime = EngineTime.timePassed;     

        // clear velocity
        blackboard.Velocity = Vector3.Zero;

        // camera follow
		blackboard.cameraSpringArm.MoveToFollowCharacter(blackboard.GlobalPosition, blackboard.Velocity);   
    }

   
   
    public override State Transition()
    {
        // check for bandage components
        var depletedComponents = !PlayerInventory.inventory.CheckInventoryForBandageComponents();

        if(depletedComponents)
        {
            // idle
            return blackboard.stateIdle;
        }


		return this;
    }
}
