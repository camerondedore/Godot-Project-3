using Godot;
using System;
using PlayerCharacterComplex;

namespace NonPlayerCharacter;

public partial class NpcMerchant : CharacterBody3D
{

    public StateMachineQueue machine = new StateMachineQueue();
    public State stateIdle,
        stateTalk,
        stateOffer,
        stateSell,
        stateCancel,
        stateTurn;

    [Export]
    public AnimationPlayer animation;
    [Export]
    public string idleAnimationName,
        talkAnimationName,
        turnRightAnimationName,
        turnLeftAnimationName,
        giveAnimationName;
    [Export]
    public float lookTime = 1f;
    [Export]
    public int price = 15;

    public Area3D offerTriggerArea,
        crierTriggerArea;
    public NpcCameraControl cameraControl;
    public NpcDialogue dialogue;
    public PlayerCharacter player;
    public RigidbodySpawner itemSpawner;
    public Vector3 initLookDirection,
        startLookDirection,
        targetLookDirection;
    public CanvasLayer merchantUi;
    public Button yesButton,
        noButton;
    public float lookCursor,
        cursorTimeMultiplier;
    public bool bodyInTrigger;
    



    public override void _Ready()
    {            
        // get nodes
        offerTriggerArea = (Area3D) GetNode("OfferTriggerArea");
        crierTriggerArea = (Area3D) GetNode("CrierTriggerArea");
        cameraControl = (NpcCameraControl) GetNode("NpcCameraControl");
        dialogue = (NpcDialogue) GetNode("Dialogue");
        merchantUi = (CanvasLayer) GetNode("MerchantUi");
        yesButton = (Button) GetNode("MerchantUi/Background/YesButton");
        noButton = (Button) GetNode("MerchantUi/Background/NoButton");
        itemSpawner = (RigidbodySpawner) GetNode("ItemSpawner");

        merchantUi.Visible = false;

        initLookDirection = -Basis.Z;

        // set up events
        offerTriggerArea.BodyEntered += TriggerOffer;
        offerTriggerArea.BodyExited += OfferTriggerReset;
        yesButton.Pressed += AcceptTrade;
        noButton.Pressed += CancelTrade;

        // initialize states
        stateIdle = new NpcMerchantStateIdle(){blackboard = this};
        stateTalk = new NpcMerchantStateTalk(){blackboard = this};
        stateTurn = new NpcMerchantStateTurn(){blackboard = this};
        stateOffer = new NpcMerchantStateOffer(){blackboard = this};
        stateSell = new NpcMerchantStateSell(){blackboard = this};
        stateCancel = new NpcMerchantStateCancel(){blackboard = this};

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



    public void TriggerOffer(Node3D body)
    {
        if(bodyInTrigger == false && machine.CurrentState == stateIdle)
        {
            if(body is PlayerCharacter playerBody)
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



    public void OfferTriggerReset(Node3D body)
    {
        bodyInTrigger = false;
    }



    public void AcceptTrade()
    {
        // sell
        machine.SetState(stateSell);
    }



    public void CancelTrade()
    {
        // canel
        machine.SetState(stateCancel);
    }
}