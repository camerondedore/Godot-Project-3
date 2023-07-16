using Godot;
using System;
using System.Collections.Generic;

public partial class MobFaction : Node
{

    public static List<Node> mobs;
    
    public enum Faction 
    {
        Gaia,
        Friend,
        Enemy
    }

    [Export]
    public Faction myFaction;



    public override void _EnterTree()
    {
        mobs.Add(this);
    }



    public override void _ExitTree()
    {
        mobs.Remove(this);
    }
}
