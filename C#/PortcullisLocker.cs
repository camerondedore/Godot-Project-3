using Godot;
using System;

public partial class PortcullisLocker : Node, IActivatable
{

    [Export]
    Portcullis portcullis;



    public void Activate()
    {
        portcullis.lockedOpen = true;
    }



    public void Deactivate()
    {
        portcullis.lockedOpen = false;
    }
}