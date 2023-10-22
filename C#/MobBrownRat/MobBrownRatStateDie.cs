using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateDie : MobBrownRatState
    {





        public override void RunState(double delta)
        {

        }
        
        
        
        public override void StartState()
        {
            // get allies
            var allies = blackboard.detection.GetAllies(blackboard.maxSightRangeForAlliesSqr);

            // alert nearby allies that this mob died
            foreach(MobFaction ally in allies)
            {
                var allyBase = (IMobAlly) ally.Owner;
                allyBase.AllyKilled();
            }

            //blackboard.QueueFree();

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-die");
            blackboard.animStateMachinePlayback.Next();

            // disable mob
            blackboard.machine.Disable();
            //blackboard.collider.Disabled = true;
            //blackboard.eyes.QueueFree();
            blackboard.ProcessMode = Node.ProcessModeEnum.Disabled;
            
            // stop moving
            //blackboard.moving = false;
            //blackboard.lookAtTarget = false;
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            return this;
        }
    }
}