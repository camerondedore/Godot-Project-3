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



	public override void _EnterTree()
	{
		mobs.Add(this);
	}



	public override void _ExitTree()
	{
		mobs.Remove(this);
	}
}
