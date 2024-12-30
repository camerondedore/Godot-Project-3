using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcDialogueStateWait : NpcDialogueState
    {





        public override void StartState()
        {
            blackboard.waiting = true;
        }



        public override void EndState()
        {
            blackboard.waiting = false;
        }
    }
}