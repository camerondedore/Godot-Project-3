using Godot;
using System;

public partial class PlayerCharacterStateFall : PlayerCharacterState
{





    public override void RunState(double delta)
    {
       // get input
		var moveDirection = blackboard.GetMoveInput();

		// set up velocity using input
        var vel = blackboard.Velocity;
		vel.X = Mathf.Lerp(vel.X, moveDirection.X * blackboard.speed, ((float) delta) * blackboard.acceleration);
		vel.Z = Mathf.Lerp(vel.Z, moveDirection.Z * blackboard.speed, ((float) delta) * blackboard.acceleration);

        // apply gravity
        //blackboard.ySpeed += Gravity.vector.Y * ((float) delta);

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
        //blackboard.ySpeed = blackboard.Velocity.Y;
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        if(blackboard.IsOnFloor())
		{
			// land
			//return blackboard.stateLand;
            return blackboard.stateIdle;
		}

		return this;
    }
}