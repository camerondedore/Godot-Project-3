using Godot;
using System;

namespace PlayerBow
{
    public partial class Bow : Node3D
    {

    [Export]
    PackedScene weightedArrow,
        pickArrow,
        netArrow,
        fireArrow,
        bodkinArrow,
        bladeArrow;
    [Export]
    AudioTools3d bowAudio;
    [Export]
    AudioStream bowFireSound,
        bowDrawSound;

    public bool isDrawn = false;



    public bool Fire(IBowTarget target)
    {
            isDrawn = false;

            // check for target
            if(target == null)
            {
                GD.Print("null bow target");
                return false;
            }

            var arrowToFire = GetArrowPrefab(target.GetArrowType());

            // create new arrow
            var newArrow = (Arrow) arrowToFire.Instantiate();
            
            // set new arrow position and look direction
            var direction = GetLaunchVectorToHitTarget(GlobalPosition, target.GetTargetGlobalPosition(), newArrow.speed);

            if(direction == Vector3.Zero)
            {
                return false;
            }

            newArrow.LookAtFromPosition(GlobalPosition, GlobalPosition + direction.Normalized());
            
            // assign to scene
            GetTree().CurrentScene.AddChild(newArrow);
            newArrow.Owner = GetTree().CurrentScene;

            // play audio
            bowAudio.PlaySound(bowFireSound, 0.05f);

            return true;
        }



        PackedScene GetArrowPrefab(string arrowType)
        {
            // default weighted
            var arrowToFire = weightedArrow;

            switch(arrowType)
            {
                case "pick":
                    arrowToFire = pickArrow;
                    break;
                case "net":
                    arrowToFire = netArrow;
                    break;
                case "fire":
                    arrowToFire = fireArrow;
                    break;
                case "bodkin":
                    arrowToFire = bodkinArrow;
                    break;
                case "blade":
                    arrowToFire = bladeArrow;
                    break;
            }

            return arrowToFire;
        }



        public void Draw()
        {
            if(isDrawn)
            {
                return;            
            }

            isDrawn = true;

            // play audio
            bowAudio.PlaySound(bowDrawSound, 0.05f);
        }



        public void CancelDraw()
        {
            isDrawn = false;

            // play audio
            bowAudio.Stop();
        }



        public Vector3 GetLaunchVectorToHitTarget(Vector3 start, Vector3 target, float speed)
        {            
            // get vector to target
            var direction = target - start;

            // get components of vector to target
            var flatDirection = direction;
            flatDirection.Y = 0;

            var x = flatDirection.Length();
            var y = direction.Y;

            // theta = atan( (s^2 +/- sqrt( s^4 - g(g*x^2 - 2*s^2*y) )) / (g*x) )
            // get launch angle
            var gravity = -EngineGravity.magnitude;

            var speedSquared = Mathf.Pow(speed, 2);
            var speedQuad = Mathf.Pow(speed, 4);
            var gravityAgainstSpeed = gravity * (gravity * Mathf.Pow(x, 2) - 2 * speedSquared * y);
            var top = -speedSquared + Mathf.Sqrt(speedQuad - gravityAgainstSpeed);
            var bottom = gravity * x;


            if(speedQuad < gravityAgainstSpeed)
            {
                GD.Print("arrow cannot hit target: " + speedQuad + " - " + gravityAgainstSpeed + " is negative");
                
                // arrow cannot hit target
                return Vector3.Zero;
            }

            var angle = Mathf.Atan(top / bottom);

            // assemble vector components
            var vXZ = speed * Mathf.Cos(angle);
            var vY = speed * Mathf.Sin(angle);
            
            // create launch vector
            var launchVelocity = flatDirection.Normalized();
            launchVelocity.X *= vXZ;
            launchVelocity.Z *= vXZ;
            launchVelocity.Y = vY;
            
            return launchVelocity;
        }



        public bool ArrowCanHitTarget(Vector3 start, Vector3 target, string arrowType)
        {
            var arrow = GetArrowPrefab(arrowType);

            // create new arrow
            var newArrow = (Arrow) arrow.Instantiate();

            var speed = newArrow.speed;

            newArrow.QueueFree();

            // get vector to target
            var direction = target - start;

            // get components of vector to target
            var flatDirection = direction;
            flatDirection.Y = 0;

            var x = flatDirection.Length();
            var y = direction.Y;

            // theta = atan( (s^2 +/- sqrt( s^4 - g(g*x^2 - 2*s^2*y) )) / (g*x) )
            // get launch angle
            var gravity = -EngineGravity.magnitude;

            var speedSquared = Mathf.Pow(speed, 2);
            var speedQuad = Mathf.Pow(speed, 4);
            var gravityAgainstSpeed = gravity * (gravity * Mathf.Pow(x, 2) - 2 * speedSquared * y);

            if(speedQuad < gravityAgainstSpeed)
            {            
                // arrow cannot hit target
                return false;
            }

            return true;
        }



        public bool TargetIsInFrontOfBow(Vector3 target)
        {
            return ToLocal(target).Z < 0.25f;
        }
    }
}