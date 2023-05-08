#if TOOLS
using Godot;
using System;

[Tool]
public partial class PluginToolPropPainter : EditorPlugin
{





    public override void _EnterTree()
    {
        var script = GD.Load<Script>("res://addons/PropPainter/ToolPropPainter.cs");
        var icon = GD.Load<Texture2D>("res://addons/PropPainter/icon.svg");

		AddCustomType("ToolPropPainter", "Node", script, icon);
    }



	public override void _ExitTree()
	{
		RemoveCustomType("PropPainter");
	}



	public override bool _Handles(GodotObject obj)
	{
		return obj is ToolPropPainter;
	}



	// public override int _Forward3DGuiInput(Camera3D cam, InputEvent e)
	// {
	// 	GD.Print("plugin input " + e.GetType());

	// 	propagate

	// 	var arguements = new Godot.Collections.Array();
	// 	arguements.Add(e);

	// 	GetEditorInterface().GetEditedSceneRoot().PropagateCall("_Input", arguements);
	// }
}
#endif
