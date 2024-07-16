using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateDie : PlayerCharacterState
    {





        public override void RunState(double delta)
        {
            // set up velocity using input
            var vel = blackboard.Velocity;

            // apply gravity
            vel += EngineGravity.vector * ((float) delta);

            // apply velocity
            blackboard.Velocity = vel;

            blackboard.MoveAndSlide();

            // camera follow
            blackboard.cameraController.MoveToFollowCharacter(blackboard.verticalSpringArmTarget.GlobalPosition, blackboard.Velocity);
        }



        public override void StartState()
        {
            // destroy faction nodes
            foreach(var faction in blackboard.myFactions)
            {
                faction.QueueFree();
            }

            // clear velocity
            blackboard.Velocity = Vector3.Zero;

            // stop overriding back bone
            blackboard.backBone.OverridePose = false;

            // animation
            blackboard.animStateMachinePlayback.Travel("character-die");
        }




    
    
        public override State Transition()
        {
            return this;
        }
    }
}