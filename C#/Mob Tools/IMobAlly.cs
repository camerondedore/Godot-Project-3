using Godot;
using System;

public interface IMobAlly
{
    void AllyKilled();
    void AllySpottedEnemy(MobFaction enemy);
}