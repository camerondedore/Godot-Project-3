using Godot;
using System;
using System.Collections.Generic;

public partial class MobFaction : Node3D
{

	public static List<MobFaction> mobs = new List<MobFaction>();
	
	public enum Faction 
	{
		Gaia,
		Friend,
		Enemy
	}

	[Export]
	public Faction faction;
	[Export]
	public float MobPriority = 1; // high number means lower priority



	public override void _EnterTree()
	{
		mobs.Add(this);
	}



	public override void _ExitTree()
	{
		mobs.Remove(this);
	}
}
