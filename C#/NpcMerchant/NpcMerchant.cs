using Godot;
using System;
using PlayerCharacterComplex;
using Dialogue;
using System.Net.Http.Headers;

namespace NonPlayerCharacter;

public partial class NpcMerchant : CharacterBody3D
{

    public StateMachineQueue machine = new StateMachineQueue();
    public State stateIdle,
        stateTalk,
        stateCrier,
        stateOffer,
        stateSell,
        stateCancel,
        stateTurn,
        stateNoInventory;

    [Export]
    public AnimationPlayer animation;
    [Export]
    public string idleAnimationName,
        talkAnimationName,
        crierAnimationName,
        turnRightAnimationName,
        turnLeftAnimationName,
        giveAnimationName;
    [Export]
    public float lookTime = 1f;
    [Export]
    public int price = 15;
    [Export]
    public string inventory = "dock leaves";

    public Area3D offerTriggerArea,
        crierTriggerArea;
    public NpcCameraControl cameraControl;
    public NpcDialogue dialogue;
    public PlayerCharacter player;
    public RigidbodySpawner itemSpawner;
    public NpcDialogueLine offerDialogueLine,
        crierDialogueLine,
        noInventoryDialogueLine;
    public Vector3 initLookDirection,
        startLookDirection,
        targetLookDirection;
    public CanvasLayer merchantUi;
    public Button yesButton,
        noButton;
    public Label playerCandiedNutsCounter;
    public string stateAfterTurn;
    public float lookCursor,
        cursorTimeMultiplier;
    public bool bodyInOfferTrigger,
        bodyInCrierTrigger;
    
    double lastCrierTime = -15.0;
    



    public override void _Ready()
    {            
        // get nodes
        offerTriggerArea = (Area3D) GetNode("OfferTriggerArea");
        crierTriggerArea = (Area3D) GetNode("CrierTriggerArea");
        cameraControl = (NpcCameraControl) GetNode("NpcCameraControl");
        dialogue = (NpcDialogue) GetNode("Dialogue");
        merchantUi = (CanvasLayer) GetNode("MerchantUi");
        yesButton = (Button) GetNode("MerchantUi/Container/Background/YesButton");
        noButton = (Button) GetNode("MerchantUi/Container/Background/NoButton");
        playerCandiedNutsCounter = (Label) GetNode("MerchantUi/Container/CandiedNuts/CandiedNutsCounter");
        itemSpawner = (RigidbodySpawner) GetNode("ItemSpawner");
        offerDialogueLine = (NpcDialogueLine) GetNode("DialogueLineOffer");
        crierDialogueLine = (NpcDialogueLine) GetNode("DialogueLineCrier");
        noInventoryDialogueLine = (NpcDialogueLine) GetNode("DialogueLineNoInventory");

        merchantUi.Visible = false;

        initLookDirection = -Basis.Z;

        // set up events
        offerTriggerArea.BodyEntered += TriggerOffer;
        offerTriggerArea.BodyExited += OfferTriggerReset;
        crierTriggerArea.BodyEntered += TriggerCrier;
        crierTriggerArea.BodyExited += CrierTriggerReset;
        yesButton.Pressed += AcceptTrade;
        noButton.Pressed += CancelTrade;

        // initialize states
        stateIdle = new NpcMerchantStateIdle(){blackboard = this};
        stateTalk = new NpcMerchantStateTalk(){blackboard = this};
        stateCrier = new NpcMerchantStateCrier(){blackboard = this};
        stateTurn = new NpcMerchantStateTurn(){blackboard = this};
        stateOffer = new NpcMerchantStateOffer(){blackboard = this};
        stateSell = new NpcMerchantStateSell(){blackboard = this};
        stateCancel = new NpcMerchantStateCancel(){blackboard = this};
        stateNoInventory = new NpcMerchantStateNoInventory(){blackboard = this};

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
        if(bodyInOfferTrigger == false)
        {
            // check inventory
            if(CheckInventory(inventory) == false)
            {
                if(machine.CurrentState == stateNoInventory)
                {
                    bodyInOfferTrigger = true;

                    return;
                }

                // set look target
                targetLookDirection = body.GlobalPosition - GlobalPosition;
                targetLookDirection.Y = 0;

                stateAfterTurn = "noInventory";

                // turn state
                machine.SetState(stateTurn);
                
                bodyInOfferTrigger = true;

                return;
            }


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

            stateAfterTurn = "talk";

            // turn state
            machine.SetState(stateTurn);
            
            bodyInOfferTrigger = true;
        }
    }



    public void EndDialogue()
    {
        // set player to idle state
        player.SetToIdle();
    }



    public void OfferTriggerReset(Node3D body)
    {
        bodyInOfferTrigger = false;
    }



    public void AcceptTrade()
    {
        // sell
        machine.SetState(stateSell);
    }



    public void CancelTrade()
    {
        // cancel
        machine.SetState(stateCancel);
    }



    public void TriggerCrier(Node3D body)
    {
        // check inventory
        if(CheckInventory(inventory) == false)
        {
            bodyInCrierTrigger = true;

            // no inventory, do not go to crier state
            return;
        }

        var randomChanceNumber = GD.Randi() % 2;
        
        // 50% chance of not going to crier state
        if(randomChanceNumber == 1)
        {
            bodyInCrierTrigger = true;
            return;
        }

        if(EngineTime.timePassed < lastCrierTime + 15.0)
        {
            // too soon to go to crier state
            bodyInCrierTrigger = true;
            return;
        }
        
        if(bodyInCrierTrigger == false && machine.CurrentState == stateIdle && dialogue.waiting == true)
        {
            lastCrierTime = EngineTime.timePassed;

            // crier
            machine.SetState(stateCrier);

            bodyInCrierTrigger = true;
        }
    }



    public void CrierTriggerReset(Node3D body)
    {
        bodyInCrierTrigger = false;
    }



    public bool CheckInventory(string itemName)
    {
        bool hasInventory = false;

        switch(itemName)
        {
            case "dock leaves":
                hasInventory = true;
                break;
            case "sanicle":
                hasInventory = true;
                break;
            case "armor":
                hasInventory = !PlayerStatistics.statistics.AtMaxArmor();
                break;
            case "cider":
                hasInventory = !PlayerStatistics.statistics.AtMaxHitPointUpgrades();
                break;
        }

        return hasInventory;
    }
}