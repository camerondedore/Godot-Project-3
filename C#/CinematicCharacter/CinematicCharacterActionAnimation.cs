using Godot;
using System;
using CinematicSimple;

namespace CinematicCharacter
{
    public partial class CinematicCharacterActionAnimation : Node3D, CinematicSimpleControl.iCinematicSimpleAction
    {

        [Export]
        CinematicCharacter targetCharacter;
        [Export]
        string nextAnimation = "";


        public void PlayCinematicAction()
        {
            targetCharacter.SetTargetAnimation(nextAnimation);
        }
    }
}