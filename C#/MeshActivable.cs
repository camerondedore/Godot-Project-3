using Godot;
using System;

public partial class MeshActivable : MeshInstance3D, IActivatable
{





    public void Activate()
    {
        Visible = false;
    }



    public void Deactivate()
    {
        Visible = false;
    }
}