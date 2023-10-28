using Godot;
using System;

public interface IMobAlly
{
    void AllyHurt();
    void AllySpottedEnemy(MobFaction enemy);
}