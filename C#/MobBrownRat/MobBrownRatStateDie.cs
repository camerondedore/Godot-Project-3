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
                // temporary casting; may convert to interface later
                var allyBase = (IMobAlly) ally.Owner;
                allyBase.AllyKilled();
            }

            blackboard.QueueFree();

            // animation
            //blackboard.animation.Set("parameters/conditions/die", true);
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