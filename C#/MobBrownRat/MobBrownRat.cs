using Godot;
using System;


namespace MobBrownRat
{
    public partial class MobBrownRat : CharacterBody3D
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateIdle,
            stateMove,
            stateFlee,
            stateAim,
            stateFire,
            stateDodge,
            stateCooldown,
            stateRetreat,
            stateDie;

        [Export]
        public MobDetection detection;
        [Export]
        NavigationAgent3D nav;
        [Export]
        public float maxSightRangeSqr = 100,
            maxSightRangeForAlliesSqr = 100,
            maxMoveRangeSqr = 1600,
            attackDistanceSqr = 225,
            fleeDistanceSqr = 25,
            speed = 3,
            lookSpeed = 4,
            acceleration = 4;
        [Export]
        bool defensive = false;

        public MobFaction enemy;
        public Vector3 targetPosition;

        bool delay = false;


        public override void _Ready()
        {
            targetPosition = GlobalPosition;
            
            // initialize states
            stateIdle = new MobBrownRatStateIdle(){blackboard = this};
            stateMove = new MobBrownRatStateMove(){blackboard = this};
            stateFlee = new MobBrownRatStateFlee(){blackboard = this};
            stateAim = new MobBrownRatStateAim(){blackboard = this};
            stateFire = new MobBrownRatStateFire(){blackboard = this};
            stateDodge = new MobBrownRatStateDodge(){blackboard = this};
            stateCooldown = new MobBrownRatStateCooldown(){blackboard = this};
            stateRetreat = new MobBrownRatStateRetreat(){blackboard = this};
            stateDie = new MobBrownRatStateDie(){blackboard = this};

            // set first state in machine
            machine.SetState(stateIdle);
        }



        public override void _EnterTree()
        {
            machine.Enable();
        }



        public override void _ExitTree()
        {
            machine.Disable();
        }



        public override void _PhysicsProcess(double delta)
        {
            // one-frame delay
            if(delay == false)
            {
                delay = true;
                return;
            }

            // test
            if(targetPosition.DistanceSquaredTo(GlobalPosition) < 2)
            {
                // get new test position
                targetPosition = GlobalPosition + new Vector3((GD.Randf() - 0.5f) * 10, 0, (GD.Randf() - 0.5f) * 10);
                nav.TargetPosition = targetPosition;
            }

            // get new velocity
            var newVelocity = nav.GetNextPathPosition() - GlobalPosition;
            newVelocity.Normalized();
            newVelocity *= speed;

            // apply movement
            Velocity = Velocity.Lerp(newVelocity, acceleration * ((float) delta));
            MoveAndSlide();

            if(targetPosition != GlobalPosition)
            {
                // apply look
                LookAt(targetPosition);
            }
        }
    }
}