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
            return blackboard.stateMove;
		}

		return this;
    }
}