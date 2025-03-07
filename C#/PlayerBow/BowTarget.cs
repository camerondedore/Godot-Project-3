using Godot;
using System;

namespace PlayerBow
{
    public partial class BowTarget : Node3D, IBowTarget
    {

        [Export]
        string arrowType = "weighted";



        public string GetArrowType()
        {
            return arrowType;
        }



        public void Hit(Vector3 dir)
        {
            QueueFree();
        }



        public Vector3 GetTargetGlobalPosition()
        {
            if(IsInstanceValid(this))
            {
                return GlobalPosition;
            }
            else
            {
                return Vector3.Zero;
            }
        }
    }
}