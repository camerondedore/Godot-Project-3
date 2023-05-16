using System.Collections;
using System.Collections.Generic;
using Godot;

public class TimedDisconnector
{

	public float releaseTime = 1;
	double hookTime = -1000;



	/// <summary>
	/// Trips the disconnector and returns if the action can happen.
	/// </summary>
	public bool Trip(float value)
	{
		// get if cycle is finished
		var cycleFinished = EngineTime.timePassed > hookTime + releaseTime;

		if(cycleFinished && value > 0)
		{
			// auto sear is not engaged and can trip
			hookTime = EngineTime.timePassed;
			return true;
		} 

		// auto sear is engaged or trigger isn't pulled
		return false;
	}
}
