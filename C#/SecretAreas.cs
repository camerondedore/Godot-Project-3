using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SecretAreas : Node
{

    public static List<SecretArea> secretAreas = new List<SecretArea>();

    public static event Action<string> secretAreasChanged;



    public override void _EnterTree()
    {
        secretAreas = new List<SecretArea>();
        secretAreasChanged = null;
    }



    public static void SecretAreasUpdated()
    {
        secretAreasChanged.Invoke(GetSecretAreaCounter());
    }



    public static string GetSecretAreaCounter()
    {
        // get number of secret areas discovered in the level
        var secretAreasDiscovered = secretAreas.Where(s => s.discovered == true).Count();

        return $"{secretAreasDiscovered}/{secretAreas.Count}";
    }
}