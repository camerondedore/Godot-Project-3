using Godot;
using System;

namespace LevelChange
{
    public partial class LevelChangeControl : Node
    {

        public StateMachine machine = new StateMachine();
        public State stateStart,
            stateWait,
            stateEnd;

        [Export]
        public bool saveOnEnd = true;
        [Export]
        public double transitionTime = 1;
        [Export]
        public ColorRect fadeRect;
        [Export]
        public CanvasLayer canvas;
        [Export]
        public Color blockColor,
            clearColor;
        [Export]
        public Label loadingLabel;
        
        public string nextLevel;
        public Vector3 nextCheckpointPosition = Vector3.Up,
            nextCheckpointEulerRotation = -Vector3.Forward,
            nextCameraPosition = Vector3.Up,
            nextCameraEulerRotation = Vector3.Zero;
        public bool loadSavedLevel = false;



        public override void _Ready()
        {
            // initialize states
            stateStart = new LevelChangeStateStart(){blackboard = this};
            stateWait = new LevelChangeStateWait(){blackboard = this};
            stateEnd = new LevelChangeStateEnd(){blackboard = this};

            // set first state in machine
            machine.SetState(stateStart);
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



        public void ChangeLevel(string newLevel)
        {
            nextLevel = newLevel;

            machine.SetState(stateEnd);
        }



        public void ChangeLevel(string newLevel, Vector3 newCheckpointPosition, Vector3 newCheckpointEulerRotation, Vector3 newCameraPosition, Vector3 newCameraEulerRotation)
        {
            nextLevel = newLevel;
            nextCheckpointPosition = newCheckpointPosition;
            nextCheckpointEulerRotation = newCheckpointEulerRotation;
            nextCameraPosition = newCameraPosition;
            nextCameraEulerRotation = newCameraEulerRotation;

            machine.SetState(stateEnd);
        }



        public void LoadLevel(string defaultLevel)
        {
            nextLevel = defaultLevel;
            nextCheckpointPosition = Vector3.One;
            nextCheckpointEulerRotation = Vector3.Zero;
            nextCameraPosition = Vector3.One;
            nextCameraEulerRotation = Vector3.Zero;
            loadSavedLevel = true;

            machine.SetState(stateEnd);
        }
    }
}