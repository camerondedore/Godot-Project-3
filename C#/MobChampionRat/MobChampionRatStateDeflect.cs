using Godot;
using System;

namespace MobChampionRat;

public partial class MobChampionRatStateDeflect : MobChampionRatState
{

    double startTime;



    public override void StartState()
    {
        startTime = EngineTime.timePassed;

        // freeze rat
        blackboard.lookAtTarget = false;
        blackboard.moving = false;

        // make rat protected against attack
        blackboard.vulnerable = false;

        // turn rat towards arrow
        var lookTarget = blackboard.GlobalPosition - blackboard.arrowHitDirection;
        lookTarget.Y = blackboard.GlobalPosition.Y;
        blackboard.LookAt(lookTarget);

        // animation
        blackboard.animation.Play("champion-rat-deflect");

        // fx
        blackboard.brokenArrowSpawnerFx1.Spawn();
        blackboard.brokenArrowSpawnerFx2.Spawn();

        // clear head look target
        blackboard.headControl.ClearTarget();

        // aggro rat
        blackboard.AggroAllies();
        blackboard.isAggro = true;
    }



    public override void EndState()
    {
        if(blackboard.IsEnemyValid() == true)
        {
            // set head look target
            blackboard.headControl.SetTarget(blackboard.enemy);
        }
    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + blackboard.deflectAnimationTime)
        {
            if(blackboard.IsEnemyValid() == true)
            {
                // move
                return blackboard.stateMove;
            }
            else
            {
                // react
                return blackboard.stateReact;
            }
        }

        return this;
    }
}