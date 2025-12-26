using Godot;
using NonPlayerCharacter;
using System;

public partial class NpcSimpleVoleGuard : NpcSimple
{

    [Export]
    Color armorColor;

    string meshPath = "character-vole-guard/character-armature/Skeleton3D/character-vole-guard";



    public override void _Ready()
    {
        base._Ready();

        var meshNode = (MeshInstance3D) GetNode(meshPath);

        // set material armor color
        meshNode.GetSurfaceOverrideMaterial(0).Set("shader_parameter/armorColor", armorColor);
    }
}