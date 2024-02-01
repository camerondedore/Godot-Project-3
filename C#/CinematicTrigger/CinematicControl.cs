using Godot;
using System;
using PlayerCharacterComplex;

namespace Cinematic
{
    public partial class CinematicControl : Node3D
    {

        public StateMachine machine = new StateMachine();
        public State stateStart,
            stateTransition,
            stateWait,
            stateEnd;

        [Export]
        public CinematicTarget[] targets;
        [Export]
        public bool saveToWorldData = true;
        [Export]
        public double startDelay = 0;
        
        public PlayerCharacter player;
        public int targetIndex = 0;



        public override void _Ready()
        {
            if(saveToWorldData)
            {
                // get if trigger was already activated
                var wasActivated = WorldData.data.CheckActivatedObjects(this);

                if(wasActivated)
                {
                    // clear targets
                    foreach(var target in targets)
                    {
                        target.QueueFree();
                    }

                    // destroy trigger
                    QueueFree();  
                    
                    return;
                }
            }

            // initialize states
			stateStart = new CinematicStateStart(){blackboard = this};
			stateTransition = new CinematicStateTransition(){blackboard = this};
            stateWait = new CinematicStateWait(){blackboard = this};
            stateEnd = new CinematicStateEnd(){blackboard = this};
        }



        public override void _Process(double delta)
        {
            // run machine
			if(machine != null && machine.CurrentState != null)
			{
				machine.CurrentState.RunState(delta);
				machine.SetState(machine.CurrentState.Transition());
			}
        }



        public void Triggered(Node3D body)
        {
            // check that body is jump pad user
            if(body is PlayerCharacter)
            {
                player = body as PlayerCharacter;

                // set first state in machine
			    machine.SetState(stateStart);                
            }
        }
    }
}