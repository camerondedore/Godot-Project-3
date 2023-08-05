using Godot;
using System;
using System.Collections.Generic;

namespace PlayerBow
{
    public partial class BowAimerTargetFx : Sprite3D
    {

        [Export]
        Texture2D weightedSprite,
            pickSprite,
            netSprite,
            fireSprite;
        [Export]
        float scaleSpeed = 0.5f,
            scaleRadius = 0.1f;

        IBowTarget activeTarget;



        public override void _Ready()
        {
            TopLevel = true;
        }



        public override void _Process(double delta)
        {
            if(activeTarget != null)
            {
                // move fx to target
                GlobalPosition = activeTarget.GetGlobalPosition();

                Scale = Vector3.One * (1 + ((float) Mathf.Sin(EngineTime.timePassed * scaleSpeed)) * scaleRadius);
            }
        }



        public void HasTarget(IBowTarget target)
        {
            activeTarget = target;
            Visible = true;        
        
            // default to weighted
            var newTexture = weightedSprite;

            switch(activeTarget.GetArrowType())
            {   
                case "pick":
                    newTexture = pickSprite;
                    break;
                case "net":
                    newTexture = netSprite;
                    break;
                case "fire":
                    newTexture = fireSprite;
                    break;
            }

            Texture = newTexture;
        }



        public void NoTarget()
        {
            Visible = false;
            activeTarget = null;
        }
    }
}