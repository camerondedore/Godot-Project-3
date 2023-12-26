using Godot;
using System;
using System.Data.Common;

namespace MobBlackRat
{
    public partial class MobBlackRatStateAttack : MobBlackRatState
    {

        double startTime;
        bool damageOutputted = false;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.LookForEnemy();

            
            // check for timing to output damage
            if(damageOutputted == false && EngineTime.timePassed > startTime + blackboard.attackDamageTime)
            {
                // get distance to enemy
                var distanceToEnemySqr = blackboard.GetDistanceSqrToEnemy();
                var angleToEnemy = blackboard.GetForwardToEnemyAngle();
                if(distanceToEnemySqr < blackboard.attackRangeSqr && angleToEnemy < blackboard.attackAngle)
                {
                    // apply damage
                    
                }

                damageOutputted = true;
            }
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;
            
            // stop moving
            blackboard.moving = false;
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for no enemy
            if(blackboard.IsEnemyValid() == false)
            {
                // reset brown rat aggro
                blackboard.isAggro = false;

                // cool down
                return blackboard.stateCooldown;
            }
            

            // check if attack is over
            if(EngineTime.timePassed > startTime + blackboard.swingTime)
            {
                // move
                return blackboard.stateMove;
            }

            return this;
        }
    }
}