using Godot;
using System;

public partial class PlayerCharacterStateJump : PlayerCharacterState
{





    public override void RunState(double delta)
    {
        // get input
		var moveDirection = Vector3.Zero;
		moveDirection.X = PlayerInput.move.X;
		moveDirection.Z = PlayerInput.move.Z;
		
        // convert move direction to local space
        moveDirection = GlobalCamera.camera.ToGlobal(moveDirection) - GlobalCamera.camera.Position;

        moveDirection = moveDirection.Normalized();


		// set up velocity using input
        var vel = blackboard.Velocity;
		vel.X = Mathf.Lerp(vel.X, moveDirection.X * blackboard.speed, ((float) delta) * blackboard.acceleration);
		vel.Z = Mathf.Lerp(vel.Z, moveDirection.Z * blackboard.speed, ((float) delta) * blackboard.acceleration);


        // apply gravity
        vel.Y = blackboard.Velocity.Y + Gravity.vector.Y * ((float) delta);


        // apply velocity
		blackboard.Velocity = vel;

        blackboard.MoveAndSlide();

        blackboard.CharacterLook();

        // camera follow
		blackboard.cameraSpringArm.MoveToFollowCharacter(blackboard.GlobalPosition, blackboard.Velocity);
    }



    public override void StartState()
    {
        var vel = blackboard.Velocity;

        // set vertical speed; v = (-2hg)>(1/2)
		vel.Y = Mathf.Sqrt((-2 * blackboard.jumpHeight * -Gravity.magnitude));

        blackboard.Velocity = vel;
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        if(blackboard.Velocity.Y <= 0 && !blackboard.IsOnFloor())
        {
            // fall
            return blackboard.stateFall;
        }

		if(blackboard.IsOnFloor())
		{
			// move
			return blackboard.stateMove;
		}

		return this;
    }
}
