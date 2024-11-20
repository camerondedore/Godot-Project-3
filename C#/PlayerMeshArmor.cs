using Godot;
using System;

public partial class PlayerMeshArmor : MeshInstance3D
{

    [Export]
    Color[] armorColors;



    public override void _Ready()
    {
        // set starting armor color
        UpdateArmorColor(PlayerStatistics.statistics.currentStatistics.ArmorUpgrades);

        // set up event handler
		PlayerStatistics.statistics.ArmorUpgradesChanged += UpdateArmorColor;
    }



    public void UpdateArmorColor(int armor)
    {
        // set material armor color
        GetSurfaceOverrideMaterial(0).Set("shader_parameter/armorColor", armorColors[armor]);
    }
}
