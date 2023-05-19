using Godot;
using System;

public partial class PlayerCharacterStateLedgeGrab : PlayerCharacterState
{





    public override void RunState(double delta)
    {
        
    }



    public override void StartState()
    {
        // clear velocity
        blackboard.Velocity = Vector3.Zero;

        // set character to look into ledge
        blackboard.CharacterLook(-blackboard.ledgeDetectorRayHorizontal.GetCollisionNormal());

        // get character ledge offset position
        // ledge wall hit
        var ledgeHitPosition = blackboard.ledgeDetectorRayHorizontal.GetCollisionPoint(); 
        // ledge top height
        ledgeHitPosition.Y = blackboard.ledgeDetectorRayVertical.GetCollisionPoint().Y; 

        // adjust for rays offset from player center
        var characterOffset = blackboard.ledgeDetectorRayHorizontal.GetCollisionNormal() * blackboard.ledgeGrabOffset.Z;
        characterOffset.Y = blackboard.ledgeGrabOffset.Y;
        
        var ledgeGrapPosition = ledgeHitPosition + characterOffset;
        
        // apply ledge grap position to character
        blackboard.GlobalPosition = ledgeGrapPosition;

        // camera follow
		blackboard.cameraSpringArm.MoveToFollowCharacter(blackboard.GlobalPosition, blackboard.Velocity);
    }

   
   
    public override State Transition()
    {
        if(blackboard.jumpDisconnector.Trip(PlayerInput.jump))
        {
            // jump
            return blackboard.stateLedgeGrabJump;
        }

        // check that player input is pointing away from ledge
        var movingAwayLedge = blackboard.ledgeDetectorRayHorizontal.GetCollisionNormal().AngleTo(blackboard.GetMoveInput()) < 1.571f;
        
        if(PlayerInput.isMoving && movingAwayLedge)
        {
            // move
            return blackboard.stateMove;
        }

		return this;
    }
}
