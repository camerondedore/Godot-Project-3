using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class ToolPropPainter : Node
{

	[Export]
	bool toolActive = false;
	[Export]
	PackedScene[] propsToPaint;
	[Export]
	float positionSpread = 0.2f,
		maxRotation = 2,
		sizeVariation = 0.2f;
	[Export]
	uint mask = 1;
	Camera3D editorCamera;
	Vector3 rayHitPoint;
	double timer = 1;



	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			// get editor camera
			editorCamera = GetEditorCamera();
		}
	}



	public override void _Process(double delta)
	{	
		if (Engine.IsEditorHint() && toolActive && editorCamera != null)
		{
			// get mouse 2d position in editor camera's viewport
			var cursor2dPosition = editorCamera.GetViewport().GetMousePosition();

			// cast ray
			var spaceState = editorCamera.GetWorld3D().DirectSpaceState;
			
			var rayStart = editorCamera.ProjectRayOrigin(cursor2dPosition);
			var rayEnd = rayStart + editorCamera.ProjectRayNormal(cursor2dPosition) * 100;
			//var rayEnd = rayStart + editorCamera.Basis.Z * -100;

			var rayParams = new PhysicsRayQueryParameters3D(){From = rayStart, To = rayEnd, CollisionMask = mask};

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

			// debug ray
			//((Node3D) GetChild(0)).GlobalPosition = rayHitPoint;

			// check for trigger
			if(timer <= 0 && propsToPaint.Length > 0)
			{
				// paint prop into scene
				PaintProp(rayHitPoint);

				timer = 1;
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



	void PaintProp(Vector3 paintPoint)
	{
		// get prop to paint from array
		int propIndex = Mathf.RoundToInt(GD.Randi() % propsToPaint.Length);

		// load prop resource
		var prop = propsToPaint[propIndex].Instantiate() as Node3D;

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

		// set parent as root scene
		GetTree().EditedSceneRoot.AddChild(prop);
		prop.Owner = GetTree().EditedSceneRoot;

		// apply prop transform properties
		prop.GlobalPosition = paintPoint + spread;
		prop.RotationDegrees = rotationSpread;
		prop.Scale = sizeSpread;
	}
}