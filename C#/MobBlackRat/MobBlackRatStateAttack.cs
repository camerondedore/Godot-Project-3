using Godot;
using System;
using System.Data.Common;

namespace MobBlackRat
{
    public partial class MobBlackRatStateAttack : MobBlackRatState
    {

        double startTime;
        int lastSwingNumber = 2;
        bool damageOutputted = false;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.LookForEnemy();

            
            if(blackboard.IsEnemyValid())
            {
                // check for timing to output damage
                if(damageOutputted == false && EngineTime.timePassed > startTime + blackboard.attackDamageTime)
                {
                    // get distance to enemy
                    var distanceToEnemySqr = blackboard.GetDistanceSqrToEnemy();
                    var angleForwardToEnemy = blackboard.GetForwardToEnemyAngle();
                    var angleUpToEnemy = blackboard.GetUpToEnemyAngle();

                    var enemyInFront = distanceToEnemySqr < blackboard.attackRangeSqr && angleForwardToEnemy < blackboard.attackAngle;
                    var enemyAbove = distanceToEnemySqr < blackboard.attackRangeSqr * 1.5f && angleUpToEnemy < blackboard.attackAngle;

                    if(enemyInFront || enemyAbove)
                    {
                        // hurt enemy
                        // get health node by name, as direct child to the faction node's owner
                        var hitHealth = blackboard.enemy.Owner.GetNode<Health>("Health"); 
                        hitHealth.Damage(blackboard.damage);

                        // play hit fx
                        blackboard.swordHitFx.Restart();
                        
                        if(hitHealth.hasBlood)
                        {
                            // play blood fx
                            blackboard.swordHitBloodFx.Restart();
                        }

                        // play hit sound
                        blackboard.audio.PlaySwordHitSound();
                    }

                    damageOutputted = true;
                }
            }
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            damageOutputted = false;
            
            // stop moving
            blackboard.moving = false;

            // look at enemy
            blackboard.lookAtTarget = true;

            // animation
            if(lastSwingNumber == 2)
            {
                blackboard.animation.Play("black-rat-attack-1");
                lastSwingNumber = 1;
            }
            else if(lastSwingNumber == 1)
            {
                blackboard.animation.Play("black-rat-attack-2");
                lastSwingNumber = 2;
            }

            // play swing sound
            blackboard.audio.PlaySwordSwingSound();
        }



        public override void EndState()
        {
            // reset swings
            lastSwingNumber = 2;
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
                // get distance to enemy
                var distanceToEnemySqr = blackboard.GetDistanceSqrToEnemy();

                // check if enemy is close enough
                if(distanceToEnemySqr < blackboard.attackRangeSqr)
                {
                    // attack
                    StartState();
                    return this;
                }
                else
                {
                    // move
                    return blackboard.stateMove;
                }

            }

            return this;
        }
    }
}