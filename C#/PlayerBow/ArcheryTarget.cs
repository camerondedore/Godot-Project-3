using Godot;
using System;

namespace PlayerBow
{
    public partial class ArcheryTarget : Node3D, IBowTarget
    {

        string arrowType = "bodkin";
        Vector3 randomSpread;
        float randomRadius = 0.3f;



        public override void _Ready()
        {
            GenerateRandomSpread();
        }



        public string GetArrowType()
        {
            return arrowType;
        }



        public bool Hit(Vector3 dir)
        {
            GenerateRandomSpread();

            // do nothing else

            return false;
        }



        public Vector3 GetTargetGlobalPosition()
        {
            if(IsInstanceValid(this))
            {
                return GlobalPosition + randomSpread;
            }
            else
            {
                return Vector3.Zero;
            }
        }



        void GenerateRandomSpread()
        {
            // get random angle (radians) to create spread circle
                var randomAngle = GD.Randf() * 2f * Mathf.Pi;

                // turn angle into spread unit vector
                randomSpread = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));

                // convert spread into global direction and apply radius
                randomSpread = (ToGlobal(randomSpread) - GlobalPosition) * GD.Randf() * randomRadius;
        }
    }
}