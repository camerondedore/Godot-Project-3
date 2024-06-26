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
        public string nextLevel;
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



        public void SetMachineToEndState()
        {
            machine.SetState(stateEnd);
        }
    }
}