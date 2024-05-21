using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacter : CharacterBody3D
    {

    public StateMachine machine = new StateMachine();
        public State stateStart,
            stateIdle,
            stateMove,
            stateTalk,
            stateWave;

        [Export]
        public NavigationAgent3D navAgent;
        [Export]
        public AnimationPlayer animation;

        public Node3D targetNode;



        public override void _Ready()
        {            
            // initialize states
            stateStart = new CinematicCharacterStateStart(){blackboard = this};        
            stateIdle = new CinematicCharacterStateIdle(){blackboard = this};        
            stateMove = new CinematicCharacterStateMove(){blackboard = this};        
            stateTalk = new CinematicCharacterStateTalk(){blackboard = this};        
            stateWave = new CinematicCharacterStateWave(){blackboard = this};        

            // set first state in machine
            machine.SetState(stateStart);
        }



        public override void _PhysicsProcess(double delta)
        {
            // run machine
            if(machine != null && machine.CurrentState != null)
            {
                machine.CurrentState.RunState(delta);
                machine.SetState(machine.CurrentState.Transition());
            }
        }
    }
}