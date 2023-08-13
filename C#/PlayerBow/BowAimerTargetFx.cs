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
            fireSprite,
            bodkinSprite,
            bladeSprite;
        [Export]
        float scaleSpeed = 0.5f,
            scaleRadius = 0.1f;
        [Export]
        AudioStreamPlayer3D audio;
        [Export]
        GpuParticles3D sparkleFx;

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
                case "bodkin":
                    newTexture = bodkinSprite;
                    break;
                case "blade":
                    newTexture = bladeSprite;
                    break;
            }

            Texture = newTexture;

            // play audio
            audio.Play();

            // play fx
            sparkleFx.Restart();
        }



        public void NoTarget()
        {
            Visible = false;
            activeTarget = null;

            // stop audio
            audio.Stop();

            // stop fx
            sparkleFx.Emitting = false;
        }
    }
}