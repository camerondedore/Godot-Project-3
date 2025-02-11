using Dialogue;
using Godot;
using System.Collections.Generic;
using System;
using System.Linq;
using PlayerCharacterComplex;
using PlayerBow;

namespace NonPlayerCharacter;

public partial class NpcSimple : CharacterBody3D, IActivatable
{

    public StateMachineQueue machine = new StateMachineQueue();
    public State stateIdle,
        stateTalk,
        stateTalkRepeating,
        stateTurn;

    [Export]
    public AnimationPlayer animation;
    [Export]
    public string idleAnimationName,
        talkAnimationName,
        turnRightAnimationName,
        turnLeftAnimationName;
    [Export]
    public float lookTime = 1f;
    [Export]
    public bool saveToWorldData = false,
        freezePlayer = false;

    [Export]
    public Node[] linkedObjects;

    public Area3D triggerArea;
    public NpcCameraControl cameraControl;
    public NpcDialogue dialogue;
    public CollisionShape3D collider;
    public PlayerCharacter player;
    public Vector3 initLookDirection,
        startLookDirection,
        targetLookDirection;
    public float lookCursor,
        cursorTimeMultiplier;
    public bool bodyInTrigger;




    public override void _Ready()
    {            
        // get nodes
        triggerArea = (Area3D) GetNode("TriggerArea");
        cameraControl = (NpcCameraControl) GetNode("NpcCameraControl");
        dialogue = (NpcDialogue) GetNode("Dialogue");
        collider = (CollisionShape3D) GetNode("Collider");

        initLookDirection = -Basis.Z;

        // set up events
        triggerArea.BodyEntered += TriggerDialogue;
        triggerArea.BodyExited += TriggerReset;

        if(saveToWorldData == true)
        {
            // get if trigger was already activated
            var wasActivated = WorldData.data.CheckActivatedObjects(this);

            if(wasActivated == true)
            {
                dialogue.useRepeatingDialogue = true; 
                //ActivateLinkedNodes();
            }
        }

        // initialize states
        stateIdle = new NpcSimpleStateIdle(){blackboard = this};
        stateTalk = new NpcSimpleStateTalk(){blackboard = this};
        stateTalkRepeating = new NpcSimpleStateTalkRepeating(){blackboard = this};
        stateTurn = new NpcSimpleStateTurn(){blackboard = this};

        // set first state in machine
        machine.SetState(stateIdle);
    }



    public override void _PhysicsProcess(double delta)
    {
        if(targetLookDirection != Vector3.Zero && lookCursor <= 1.1f)
        {
            // smooth look
            var smoothtargetLookDirection = startLookDirection.Slerp(targetLookDirection, lookCursor);

            // apply look
            LookAt(GlobalPosition + smoothtargetLookDirection);

            lookCursor += ((float)delta) * cursorTimeMultiplier;
        }
    }



    public override void _EnterTree()
    {
        machine.Enable();
    }



    public override void _ExitTree()
    {
        machine.Disable();
    }



    public void TriggerDialogue(Node3D body)
    {
        if(bodyInTrigger == false && machine.CurrentState == stateIdle)
        {
            if(freezePlayer == true && body is PlayerCharacter playerBody && dialogue.useRepeatingDialogue == false)
            {
                // store hit body
                player = playerBody;

                // set player to start state
                player.machine.SetState(player.stateCinematic);
            }

            // set look target
            targetLookDirection = body.GlobalPosition - GlobalPosition;
            targetLookDirection.Y = 0;

            // repeating dialogue
            machine.SetState(stateTurn);
            
            bodyInTrigger = true;
        }
    }



    public void EndDialogue()
    {
        // set player to idle state
        player.SetToIdle();
    }



    public void TriggerReset(Node3D body)
    {
        bodyInTrigger = false;
    }



    public void ActivateLinkedNodes()
    {
        if(linkedObjects.Length > 0)
        {
            // activate pinned objects
            foreach(IActivatable i in linkedObjects)
            {
                if(IsInstanceValid((Node)i) == true)
                {
                    i.Activate();
                }
            }
        }
    }



    public void Activate()
    {
        Visible = true;
        ProcessMode = ProcessModeEnum.Inherit;
        collider.SetDeferred("disabled", false);
    }



    public void Deactivate()
    {
        Visible = false;
        ProcessMode = ProcessModeEnum.Disabled;
        collider.SetDeferred("disabled", true);
    }
}