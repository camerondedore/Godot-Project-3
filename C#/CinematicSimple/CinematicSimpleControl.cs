using Godot;
using System;
using PlayerCharacterComplex;

namespace CinematicSimple
{
    public partial class CinematicSimpleControl : AnimationPlayer
    {

        public PlayerCharacter player;



        public override void _Ready()
        {

        }



        public void EndCinematic()
        {
            if(player != null)
            {
                // set player to idle state
                player.machine.SetState(player.stateIdle);
            }
        }



        public void Triggered(Node3D body, string animationName)
        {
            // check that body is player
            if(body is PlayerCharacter)
            {
                player = body as PlayerCharacter;     

                // set player to start state
                player.startDelayUsesTime = false;
                player.machine.SetState(player.stateStart);

                // play cinematic
                Play(animationName);
            }
        }



        public interface iCinematicSimpleAction
        {
            void PlayCinematicAction();
        }
    }
}