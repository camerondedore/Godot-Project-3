using Godot;
using System;
using System.Collections.Generic;

public partial class LodManager : Node
{

    [Export] 
    int objectsPerTick = 10;
	public static List<LodObject> lodObjects = new List<LodObject>();
	Camera3D camera;
    int objectIndex = 0;



    public override void _Process(double delta)
	{
		// machines check
		if(lodObjects.Count == 0)
		{
			return;
		}

        if(camera == null)
        {
            camera = GlobalCamera.camera;
        }

		// get count of pops, maxing out at the number of machines
		int c = Mathf.Clamp(objectsPerTick, 1, lodObjects.Count);

		// run machine for this tick
		while(c > 0)
		{
			// keep index in range of machines
			if(objectIndex < lodObjects.Count)
			{
				lodObjects[objectIndex].LodCheck(camera);
			}

			// next machine index
			if(objectIndex < lodObjects.Count - 1)
			{
				// next machine
				objectIndex++;
			}
			else
			{
				// first machine
				objectIndex = 0;
			}	

			c--;
		}
	
	}
}