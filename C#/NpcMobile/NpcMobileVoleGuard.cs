using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcMobileVoleGuard : NpcMobile
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
}