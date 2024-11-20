using Godot;
using System;

public partial class ArmorPickup : PickupRigidbody
{

    [Export]
    Color[] armorColors;

    MeshInstance3D mesh;



    public override void _Ready()
    {
        base._Ready();

        mesh = (MeshInstance3D) GetNode("PickupMesh");

        // set starting armor color
        UpdateArmorColor(PlayerStatistics.statistics.currentStatistics.ArmorUpgrades);

        // set up event handler
		PlayerStatistics.statistics.ArmorUpgradesChanged += UpdateArmorColor;
    }



    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        if(PlayerStatistics.statistics.currentStatistics.ArmorUpgrades >= PlayerStatistics.statistics.maxArmorUpgrades)
        {
            // armor maxxed out
            return;
        }

        // remove event handler
		PlayerStatistics.statistics.ArmorUpgradesChanged -= UpdateArmorColor;
        
        // add armor to player statistics
        PlayerStatistics.statistics.ApplyArmorUpgrade(1);

        // play player fx
        data.playerFx.PlayArmorFx();

        // play player audio
		data.playerAudio.PlayArmorPickupSound();

        base.PickupAction(data);
    }



    public void UpdateArmorColor(int armor)
    {
        int nextArmorIndex = Mathf.Clamp(armor + 1, 0, PlayerStatistics.statistics.maxArmorUpgrades);

        // set material armor color
        mesh.GetSurfaceOverrideMaterial(0).Set("shader_parameter/armorColor", armorColors[nextArmorIndex]);
    }
}
