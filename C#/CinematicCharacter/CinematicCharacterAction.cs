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
        string nextAnimation = "";
        [Export]
        bool hideOnArrival = false;



        public void TriggerCinematicAction()
        {
            targetCharacter.SetTargetNode(this, hideOnArrival, nextAnimation);
        }
    }
}