using Godot;
using System;

public partial class PlayerCharacterStateJumpPad : PlayerCharacterState
{

    Vector3 initialVelocity;



    public override void RunState(double delta)
    {
        // get input
		var moveDirection = blackboard.GetMoveInput();

        var vel = blackboard.Velocity;
        
        // check for input
        if(moveDirection.LengthSquared() > 0)
        {
            // set up velocity by adding input to jump pad velocity
            vel.X = Mathf.Lerp(vel.X, initialVelocity.X + moveDirection.X * blackboard.speed, ((float) delta) * blackboard.acceleration * 0.25f);
            vel.Z = Mathf.Lerp(vel.Z, initialVelocity.Z + moveDirection.Z * blackboard.speed, ((float) delta) * blackboard.acceleration * 0.25f);
        }		

        // apply gravity
        //blackboard.ySpeed += Gravity.vector.Y * ((float) delta);

        vel += EngineGravity.vector * ((float) delta);


        // apply velocity
		blackboard.Velocity = vel;

        blackboard.MoveAndSlide();

        blackboard.CharacterLook();

        // camera follow
		blackboard.cameraSpringArm.MoveToFollowCharacter(blackboard.GlobalPosition, blackboard.Velocity);
    }



    public override void StartState()
    {   
        // get initial jump pad velocity
        //  to be used for limiting movement
        initialVelocity = blackboard.Velocity;

        //blackboard.ySpeed = blackboard.Velocity.Y;
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        // hit something when using jump pad
		if(blackboard.IsOnWall() && !blackboard.IsOnFloor())
		{
			// fall
			return blackboard.stateFall;
		}


        if(blackboard.IsOnFloor())
		{
			// land
			//return blackboard.stateLand;
            return blackboard.stateMove;
		}

		return this;
    }
}