using Godot;
using MobChampionRat;
using System;

namespace MobChampionRat;

public partial class MobChampionRatSuperStateIdle : MobChampionRatSuperState
{





    public override void RunState(double delta)
    {            
        // look for enemy
        blackboard.LookForEnemy();

        base.RunState(delta); 
    }
    
    
    
    public override void StartState()
    {
        // stop moving
        blackboard.moving = false;

        // set substate
        SetState(blackboard.subStateIdle);

        // start position
        blackboard.startPosition = blackboard.GlobalPosition;

        base.StartState();
    }



    public override void EndState()
    {
        base.EndState();
    }



    public override State Transition()
    {
        // check if falling
        if(blackboard.IsOnFloor() == false)
        {
            // fall
            return blackboard.stateFall;
        }
        
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