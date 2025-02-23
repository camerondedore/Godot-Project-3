using Godot;
using System;

public partial class PlayerCharacterLedgeDetector : Node3D
{

    [Export]
		public RayCast3D ledgeDetectorRayHorizontal,
			ledgeDetectorRayVertical,
			ledgeDetectorRayGap,
			ceilingDetectorRay,
            groundDetectorRay;



    public override void _Ready()
    {
        TurnOff();
    }



    public void TurnOn()
    {
        ledgeDetectorRayHorizontal.Enabled = true;
        ledgeDetectorRayVertical.Enabled = true;
        ledgeDetectorRayGap.Enabled = true;
        ceilingDetectorRay.Enabled = true;
        groundDetectorRay.Enabled = true;

        ledgeDetectorRayHorizontal.ForceRaycastUpdate();
        ledgeDetectorRayVertical.ForceRaycastUpdate();
        ledgeDetectorRayGap.ForceRaycastUpdate();
        ceilingDetectorRay.ForceRaycastUpdate();
        groundDetectorRay.ForceRaycastUpdate();
    }



    public void TurnOff()
    {
        ledgeDetectorRayHorizontal.Enabled = false;
        ledgeDetectorRayVertical.Enabled = false;
        ledgeDetectorRayGap.Enabled = false;
        ceilingDetectorRay.Enabled = false;
        groundDetectorRay.Enabled = false;
    }



    public bool DetectingValidLedge()
    {
        // check for ledge
        var detectingLedge = ledgeDetectorRayHorizontal.IsColliding() && ledgeDetectorRayVertical.IsColliding();

        // check for ledge angle
        detectingLedge = detectingLedge && ledgeDetectorRayVertical.GetCollisionNormal().AngleTo(-EngineGravity.direction) < 0.175f;

        // check for gap over ledge
        detectingLedge = detectingLedge && ledgeDetectorRayGap.IsColliding() == false;

        // check for ceiling
        var detectingCeiling = ceilingDetectorRay.IsColliding();

        // check for ground
        var detectingGround = groundDetectorRay.IsColliding();

        // check for animatable body
        var animatableBodyMoving = false;

        if(ledgeDetectorRayVertical.GetCollider() is IAnimatableBody animBody)
        {
            animatableBodyMoving = animBody.IsMoving();
        }

        return detectingLedge && !detectingCeiling && !detectingGround && !animatableBodyMoving;
    }



    public Vector3 GetLedgeWallNormal()
    {
        return ledgeDetectorRayHorizontal.GetCollisionNormal();
    }



    public Vector3 GetLedgeCornerPoint()
    {
        // ledge wall hit
        var ledgeHitPosition = ledgeDetectorRayHorizontal.GetCollisionPoint(); 
        // ledge top height
        ledgeHitPosition.Y = ledgeDetectorRayVertical.GetCollisionPoint().Y; 

        return ledgeHitPosition;
    }
}
