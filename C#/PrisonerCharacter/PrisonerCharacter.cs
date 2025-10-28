using Godot;
using System;

namespace PrisonerCharacter;

public partial class PrisonerCharacter : CharacterBody3D, IActivatable
{

    public StateMachine machine = new StateMachine();
    public State stateIdle,
        stateFreedStatic,
        stateFreedMove,
        stateWave,
        stateFlee;

    [Export]
    Node3D playerCharacter;
    [Export]
    public Node3D freedTargetNode,
        fleeTargetNode;
    [Export]
    public AnimationPlayer animation;
    [Export]
    public string idleAnimationName = "beaver-idle",
        idleScaredAnimationName = "beaver-idle-scared",
        walkAnimationName = "beaver-walk",
        runAnimationName = "beaver-run",
        waveAnimationName = "beaver-wave";
   
    [Export]
    public float runSpeed = 4f,
        //walkSpeed = 2f,
        acceleration = 10,
        lookSpeed = 5f,
        freedStaticTime = 0.75f,
        waveAnimTime = 1.13f;
    [Export]
    public bool waveWhenFreed = true;

    public NavigationAgent3D navAgent;
    public AudioTools3d voiceAudio;
    public Node3D escapeTargetNode;



    public override void _Ready()
    {            
        // get nodes
        navAgent = (NavigationAgent3D) GetNode("NavAgent");
        voiceAudio = (AudioTools3d) GetNode("VoiceAudio");

        // initialize states
        stateIdle = new PrisonerCharacterStateIdle(){blackboard = this};
        stateFreedStatic = new PrisonerCharacterStateFreedStatic(){blackboard = this};
        stateFreedMove = new PrisonerCharacterStateFreedMove(){blackboard = this};
        stateWave = new PrisonerCharacterStateWave(){blackboard = this};
        stateFlee = new PrisonerCharacterStateFlee(){blackboard = this};

        // set animation blends
        animation.SetBlendTime(idleScaredAnimationName, idleAnimationName, 0.2f);
        animation.SetBlendTime(idleAnimationName, idleScaredAnimationName, 0.2f);

        // set first state in machine
        machine.SetState(stateIdle);
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



    public void MoveToTargetNode(double delta)
    {
        // check that character is in moving state
        if(IsOnFloor())
        {
            // get new velocity
            var newVelocity = navAgent.GetNextPathPosition() - GlobalPosition;
            newVelocity = newVelocity.Normalized();
            newVelocity.Y = 0;
            newVelocity *= runSpeed;
            
            if(newVelocity.X == 0 && newVelocity.Z == 0)
            {
                // no usable velocity
                return;
            }

            // smooth movement
            Velocity = Velocity.Lerp(newVelocity, acceleration * ((float) delta));

            MoveAndSlide();

            // get direction to next path point and flatten
            var lookDirection = Velocity.Normalized();
            lookDirection.Y = 0;

            if(lookDirection.LengthSquared() > 0.1f)
            {
                var smoothLookDirection = -Basis.Z;
                smoothLookDirection = smoothLookDirection.Slerp(lookDirection, lookSpeed * ((float) delta));

                // apply look
                LookAt(GlobalPosition + smoothLookDirection);
            }    
        }
        else
        {                
            // falling
            // apply gravity
            Velocity += EngineGravity.vector * ((float) delta);                
            MoveAndSlide();
        }
    }



    // public void LookWithTargetNode(double delta)
    // {
    //     if(targetNode == null)
    //     {
    //         return;
    //     }

    //     // get direction and flatten
    //     var lookTarget = GlobalPosition + -targetNode.Basis.Z;
    //     lookTarget.Y = GlobalPosition.Y;

    //     if(lookTarget.LengthSquared() > 0.1f)
    //     {
    //         var smoothLookTarget = GlobalPosition + -Basis.Z;
    //         smoothLookTarget = smoothLookTarget.Lerp(lookTarget, lookSpeed * ((float) delta));

    //         // apply look
    //         LookAt(smoothLookTarget);
    //     }
    // }



    // public void SetTargetNode(Node3D newTarget)
    // {
    //     if(Visible == false)
    //     {
    //         ShowCharacter();
    //     }

    //     // check target against old target
    //     var targetTransformChanged = true;
        
    //     if(targetNode != null)
    //     {
    //         targetTransformChanged = (targetNode.GlobalPosition - newTarget.GlobalPosition).LengthSquared() > 0.01f;
    //     }

    //     targetNode = newTarget;

    //     if(targetTransformChanged == true)
    //     {
    //         if(machine.CurrentState != stateMove)
    //         {
    //             // change to move state
    //             machine.SetState(stateMove);
    //         }
    //         else
    //         {
    //             // restart move state
    //             stateMove.StartState();
    //         }
    //     }
    //     else
    //     {
    //         if(machine.CurrentState != stateIdle)
    //         {
    //             // change to idle state
    //             machine.SetState(stateIdle);
    //         }
    //         else
    //         {
    //             // restart idle state
    //             machine.CurrentState.StartState();
    //         }
    //     }            
    // }



    // public bool IsPlayerCharacterClose()
    // {
    //     return GlobalPosition.DistanceSquaredTo(playerCharacter.GlobalPosition) < playerCharacterCloseDistanceSqr;
    // }



    // public bool IsPlayerCharacterFar()
    // {
    //     return GlobalPosition.DistanceSquaredTo(playerCharacter.GlobalPosition) > playerCharacterFarDistanceSqr;
    // }



    public void Speak(AudioStream voiceLine)
    {
        voiceAudio.PlaySound(voiceLine, 0);
    }



    public void Activate()
    {
        // set freed state
        machine.SetState(stateFreedStatic);
    }



    public void Deactivate()
    {
        // remove prisoner
        QueueFree();
    }
}
