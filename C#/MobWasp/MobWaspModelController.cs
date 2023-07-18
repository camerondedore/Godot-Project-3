using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspModelController : Node3D
    {

        [Export]
        float speed = 3,
            randomRadius = 0.33f;

        Node3D target;


        public override void _Ready()
        {
            target = (Node3D) Owner;

            TopLevel = true;
        }



        public override void _PhysicsProcess(double delta)
        {
            var randomOffset = new Vector3(GD.Randf() - 0.5f, GD.Randf() - 0.5f, GD.Randf() - 0.5f);
            randomOffset *= randomRadius;

            GlobalPosition = GlobalPosition.Lerp(target.GlobalPosition + randomOffset, speed * ((float) delta));
        }



        public void Enable()
        {
            ProcessMode = ProcessModeEnum.Inherit;
        }



        public void Disable()
        {
            ProcessMode = ProcessModeEnum.Disabled;
        }
    }
}