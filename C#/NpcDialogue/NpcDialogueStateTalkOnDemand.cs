using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcDialogueStateTalkOnDemand : NpcDialogueState
{





    public override void RunState(double delta)
    {

    }



    public override void StartState()
    {
        // npc speak
        blackboard.Speak(blackboard.dialogueOnDemand);
    }



    public override void EndState()
    {
        blackboard.dialogueOnDemand = null;
    }



    public override State Transition()
    {
        if(blackboard.Playing == false)
        {
            // wait
            return blackboard.stateWait;
        }

        return this;
    }
}