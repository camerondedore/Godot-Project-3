using Godot;
using System;

public interface IWatcher
{

    void AddWatchable(IWatchable newWatchable);

}