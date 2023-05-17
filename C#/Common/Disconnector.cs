using System.Collections;
using System.Collections.Generic;
using Godot;

public class Disconnector
{
    
	bool hook = false;



	/// <summary>
	/// Trips the disconnector and returns if an action can happen.
	/// </summary>
	public bool Trip(float value)
	{
		if(!hook && value > 0)
		{
			// disconnector is not engaged and can trip
			hook = true;
			return true;
		}

		if(hook && value <= 0)
		{
			// release disconnector
			hook = false;
		}

		// disconnector is engaged and cannot trip again
		return false;
	}
}
