using Godot;
using System;
using CinematicSimple;

namespace CinematicCharacter
{
    public partial class CinematicCharacterAction : Node3D, CinematicSimpleControl.iCinematicSimpleAction
    {

        [Export]
        CinematicCharacter targetCharacter;



        public void PlayCinematicAction()
        {
            targetCharacter.SetTargetNode(this);
        }
    }
}