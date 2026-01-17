#if TOOLS
using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class ToolPropPainter : Node
{

	[Export]
	bool toolActive = false;
	[Export]
	float toolSpeed = 0.5f;
	[Export]
	PackedScene[] propsToPaint;
	[Export]
	float positionSpread = 0.2f,
		yOffset = 0,
		maxRotation = 2,
		sizeVariation = 0.2f;
	[Export]
	bool uniformSize = false,
		useHitNormal;
	[Export]
	string maskAsBinary = "1";
	
	Camera3D editorCamera;
	Vector3 rayHitPoint,
		rayHitNormal;
	double timer = 1;
	uint maskAsDecimal;



	public override void _Ready()
	{
		if (Engine.IsEditorHint() == true)
		{
			// get editor camera
			editorCamera = GetEditorCamera();

			// convert mask to decimal
			maskAsDecimal = Convert.ToUInt32(maskAsBinary, 2);

			// load input mapping from project
			//InputMap.LoadFromProjectSettings();
		}
	}



	public override void _Process(double delta)
	{	
		if (Engine.IsEditorHint() && toolActive && editorCamera != null)
		{
			//if(Input.IsActionJustPressed("player-jump") == true)
			if(Input.IsKeyPressed(Key.Space) == true)
			{
				// get mouse 2d position in editor camera's viewport
				var cursor2dPosition = editorCamera.GetViewport().GetMousePosition();

				// cast ray
				var spaceState = editorCamera.GetWorld3D().DirectSpaceState;
				
				var rayStart = editorCamera.ProjectRayOrigin(cursor2dPosition);
				var rayEnd = rayStart + editorCamera.ProjectRayNormal(cursor2dPosition) * 200;
				//var rayEnd = rayStart + editorCamera.Basis.Z * -100;

				var rayParams = new PhysicsRayQueryParameters3D(){From = rayStart, To = rayEnd, CollisionMask = maskAsDecimal};

				var rayResult = spaceState.IntersectRay(rayParams);

				// debug ray
				//((Node3D) GetChild(0)).Position = rayEnd;

				// check for a hit point
				if(!rayResult.ContainsKey("position"))
				{
					return;
				}

				// get ray hit point
				rayHitPoint = (Vector3) rayResult["position"];
				rayHitNormal = (Vector3) rayResult["normal"];

				// debug ray
				//((Node3D) GetChild(0)).GlobalPosition = rayHitPoint;

				// check for trigger
				if(timer <= 0 && propsToPaint.Length > 0)
				{
					// paint prop into scene
					PaintProp(rayHitPoint, rayHitNormal);

					timer = toolSpeed;
				}
			}

			timer -= delta;
		}		
	}



	// public override void _Input(InputEvent e)
	// {
	// 	GD.Print("event");
	// 	if(e is InputEventMouseButton)
	// 	{
	// 		GD.Print("input");
	// 	}
	// }



	Camera3D GetEditorCamera()
	{
		// get editor camera
		var cameras = new List<Camera3D>();

		// get children of editor main screen
		var eiChildrenArray = (new EditorScript()).GetEditorInterface().GetEditorMainScreen().GetChildren();

		// convert children array to list
		var eiChildrenList = new List<Node>();
		eiChildrenList.AddRange(eiChildrenArray);

		// recursively find editor cameras
		FindEditorCameras(eiChildrenList, cameras);

		// set editor camera
		// 0 looks to be the editor's free-look camera
		return cameras[0];
	}



	void FindEditorCameras(List<Node> nodes, List<Camera3D> cameras)
	{
		// go through children to find cameras
		foreach(var child in nodes)
		{
			// check for camera type and @ in name
			if(child is Camera3D && child.Name.ToString().Contains('@'))
			{
				cameras.Add(child as Camera3D);
			}

			// get children of node
			var childrenArray = child.GetChildren();

			// convert children array to list
			var childrenList = new List<Node>();
			childrenList.AddRange(childrenArray);

			// find cameras in node's children
			FindEditorCameras(childrenList, cameras);
		}
	}



	void PaintProp(Vector3 paintPoint, Vector3 paintNormal)
	{
		// get prop to paint from array
		var propIndex = GD.Randi() % propsToPaint.Length;

		// load prop resource
		var prop = propsToPaint[propIndex].Instantiate() as Node3D;

		prop.Name = $"{prop.Name}!{Time.GetUnixTimeFromSystem()}";

		// get position spread
		var spread = new Vector3(GD.Randf() - 0.5f, 0, GD.Randf() - 0.5f) * 2 * positionSpread;

		// get rotation spread
		var rotationSpread = new Vector3(
			(GD.Randf() - 0.5f) * 2 * maxRotation, 
			GD.Randf() * 359, 
			(GD.Randf() - 0.5f) * 2 * maxRotation);

		// get size spread
		var xzSpread = 1 + (GD.Randf() - 0.5f) * sizeVariation;
		var sizeSpread = new Vector3(xzSpread, 1 + (GD.Randf() - 0.5f) * sizeVariation, xzSpread);

		if(uniformSize == true)
		{
			sizeSpread.Y = sizeSpread.X;
		}

		// set parent as root scene
		GetParent().AddChild(prop);
		prop.Owner = GetTree().EditedSceneRoot;



		// apply prop transform properties
		prop.GlobalPosition = paintPoint + spread + Vector3.Up * yOffset;

		// use ray hit normal
		if(useHitNormal == true && rayHitNormal != Vector3.Up)
		{
			var crossRight = Vector3.Up.Cross(rayHitNormal);
			var crossForward = rayHitNormal.Cross(crossRight);

			prop.LookAtFromPosition(prop.GlobalPosition, prop.GlobalPosition + crossForward);

			prop.RotateObjectLocal(Vector3.Right, Mathf.DegToRad(rotationSpread.X));
			prop.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(rotationSpread.Y));
			prop.RotateObjectLocal(Vector3.Forward, Mathf.DegToRad(rotationSpread.Z));
		}
		else
		{
			prop.RotationDegrees = rotationSpread;
		}

		prop.Scale = sizeSpread;
	}
}
#endif