using Godot;
using System;
using Cinematic;

namespace CinematicCharacter
{
    public partial class CinematicCharacterAction : Node3D, CinematicTarget.iCinematicAction
    {

        [Export]
        CinematicCharacter targetCharacter;
        [Export]
        string nextStateName = "";
        [Export]
        bool hideOnArrival = false;



        public void TriggerCinematicAction()
        {
            if(nextStateName == "")
            {
                targetCharacter.SetTargetNode(this, hideOnArrival);
            }
            else
            {
                targetCharacter.SetState(nextStateName);
            }
        }
    }
}