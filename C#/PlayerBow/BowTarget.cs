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



        public void Hit()
        {
            QueueFree();
        }



        public Vector3 GetGlobalPosition()
        {
            try
            {
                return GlobalPosition;
            }
            catch
            {
                // target was disposed
                // nothing more to do
                return Vector3.Zero;
            }
        }
    }
}