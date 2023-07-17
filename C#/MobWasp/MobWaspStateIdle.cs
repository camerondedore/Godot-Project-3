using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateIdle : MobWaspState
    {





        public override void RunState(double delta)
        {
            MobFaction enemy = blackboard.detection.LookForEnemy();

            if(enemy != null)
            {
                blackboard.LookAt(enemy.GlobalPosition);
            }
        }



        public override void StartState()
        {
            
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