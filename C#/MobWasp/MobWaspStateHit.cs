using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateHit : MobWaspState
    {

        double startTime,
            hitFxDelay = 0.1;
        bool hit = false;


        public override void RunState(double delta)
        {
            // animation
            blackboard.animation.Set("parameters/conditions/hit", false);

            if(hit == false && EngineTime.timePassed > startTime + hitFxDelay)
            {
                // spawn fx
                var newFx = (Node3D) blackboard.waspHitFx.Instantiate();
                blackboard.GetTree().CurrentScene.AddChild(newFx);
                newFx.Owner = blackboard.GetTree().CurrentScene;
                newFx.GlobalPosition = blackboard.GlobalPosition;

                // play venom fx
                blackboard.venomFx.Restart();

                // hurt enemy
                // get health node by name, as direct child to the faction node's owner
                if(blackboard.enemy != null)
                {
                    try
                    {
                        blackboard.enemy.Owner.GetNode<Health>("Health").Damage(blackboard.damage);
                    }
                    catch
                    {
                        // enemy disposed
                    }
                }

                hit = true;
            }
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;
            
            hit = false;

            var targetOffset = new Vector3(GD.Randf() - 0.5f, GD.Randf() - 0.5f, GD.Randf() - 0.5f);

            blackboard.targetPosition = blackboard.GlobalPosition + blackboard.Basis.Z + targetOffset;

            // animation
            blackboard.animation.Set("parameters/conditions/hit", true);
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            if(EngineTime.timePassed > startTime + 0.5f)
            {
                // attack
                return blackboard.stateAttack;
            }

            return this;
        }
    }
}