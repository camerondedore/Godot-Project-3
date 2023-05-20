using Godot;
using System;

public partial class PlayerCharacterStateMove : PlayerCharacterState
{





    public override void RunState(double delta)
    {
        // get input
		var moveDirection = blackboard.GetMoveInput();

		// set up velocity using input
        var vel = blackboard.Velocity;
		vel.X = Mathf.Lerp(vel.X, moveDirection.X * blackboard.speed, ((float) delta) * blackboard.acceleration);
		vel.Z = Mathf.Lerp(vel.Z, moveDirection.Z * blackboard.speed, ((float) delta) * blackboard.acceleration);


		// apply velocity
		blackboard.Velocity = vel;

        blackboard.MoveAndSlide();

        blackboard.CharacterLook();

        // camera follow
		blackboard.cameraSpringArm.MoveToFollowCharacter(blackboard.GlobalPosition, blackboard.Velocity);
    }



    public override void StartState()
    {
        
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        if(!blackboard.IsOnFloor())
		{
			// fall
			return blackboard.stateFall;
		}

		if(blackboard.jumpDisconnector.Trip(PlayerInput.jump) && blackboard.IsOnFloor())
        {
            // jump start
            //return blackboard.stateJumpStart;
            return blackboard.stateJump;
        }

        if(PlayerInput.fire1 > 0)
        {
            // aim bow
            return blackboard.stateBowAim;
        }

		if(!PlayerInput.isMoving)
		{
			// idle
			return blackboard.stateIdle;
		}

		return this;
    }
}
