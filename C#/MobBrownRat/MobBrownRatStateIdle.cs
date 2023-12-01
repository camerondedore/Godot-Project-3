using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateIdle : MobBrownRatState
    {

        double lastAnimationTime,
            timeBetweenAnimations;
        int lastAnimation = 1,
            animationCount = 2;



        public override void RunState(double delta)
        {            
            // look for enemy
            blackboard.LookForEnemy();

            if(EngineTime.timePassed > lastAnimationTime + timeBetweenAnimations)
            {
                var nextAnimation = 1;

                // get new animation
                while(nextAnimation == lastAnimation && animationCount > 1)
                {
                    nextAnimation = (int) (1 + GD.Randi() % animationCount);
                }

                // play extra idle animation
                switch(nextAnimation)
                {
                    case 1:
                        blackboard.animStateMachinePlayback.Travel("brown-rat-idle-itch");
                        break;
                    case 2:
                        blackboard.animStateMachinePlayback.Travel("brown-rat-idle-sniff");
                        break;

                }

                lastAnimationTime = EngineTime.timePassed;
                lastAnimation = nextAnimation;
                timeBetweenAnimations = GD.Randf() * 20 + 6;        
            }
        }
        
        
        
        public override void StartState()
        {
            // stop moving
            blackboard.moving = false;

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-idle");
            //blackboard.animStateMachinePlayback.Next();

            lastAnimationTime = EngineTime.timePassed;
            lastAnimation = (int) (1 + GD.Randi() % animationCount);
            timeBetweenAnimations = GD.Randf() * 20 + 4;
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.IsEnemyValid())
            {
                // react
                return blackboard.stateReact;
            }

            if(blackboard.isAggro)
            {
                // react
                return blackboard.stateReact;
            }

            return this;
        }
    }
}