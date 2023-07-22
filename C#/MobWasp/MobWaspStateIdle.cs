using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateIdle : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxRangeForEnemies);

            // check for dead allies
            if(blackboard.allies.Count != blackboard.startinyAllyCount)
            {
                blackboard.allyDied = true;
            }
        }



        public override void StartState()
        {
            blackboard.updateLook = false;
            
            blackboard.targetPosition = blackboard.startPosition;

            // get allies
            //if(blackboard.allies.Count == 0)
            //{
            blackboard.detection.GetAllies(blackboard.maxRangeForAllies);
            blackboard.startinyAllyCount = blackboard.allies.Count;
            //}

            GD.Print("idle");
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for dead allies
            if(blackboard.allyDied)
            {
                // takeoff
                return blackboard.stateTakeoff;
            }

            // check for enemy
            if(blackboard.enemy != null)
            {
                // enemy is within range
                // takeoff
                return blackboard.stateTakeoff;
            }

            return this;
        }
    }
}