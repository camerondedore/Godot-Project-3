using Godot;
using System;
using PlayerCharacterComplex;

namespace NonPlayerCharacter;

public partial class NpcMerchant : CharacterBody3D
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
    public float lookTime = 1f,
        lookSpeed = 7f;

    public Area3D triggerArea;
    public NpcCameraControl cameraControl;
    public NpcDialogue dialogue;
    public CollisionShape3D collider;
    public bool bodyInTrigger;
    public PlayerCharacter player;
    public Vector3 initLookDirection,
        startLookDirection,
        targetLookDirection;
    public float lookCursor,
        cursorTimeMultiplier;
}