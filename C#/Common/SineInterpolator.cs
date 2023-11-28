using Godot;

public class SineInterpolator
{





    /// <summary>
    /// Custom 1D Sine Interpolation.
    /// </summary>
    public static float Interpolate(float start, float end, float cursor)
    {
        // y = 0.5 * sin(pi * (x - 0.5)) + 0.5

        var interpValue = 0.5f * Mathf.Sin(Mathf.Pi * (cursor - 0.5f)) + 0.5f;
        var direction = end - start;

        var result = start + direction * interpValue;

        return result;
    }



    /// <summary>
    /// Custom 3D Sine Interpolation.
    /// </summary>
    public static Vector3 Interpolate(Vector3 start, Vector3 end, float cursor)
    {
        // y = 0.5 * sin(pi * (x - 0.5)) + 0.5

        var interpValue = 0.5f * Mathf.Sin(Mathf.Pi * (cursor - 0.5f)) + 0.5f;
        var direction = end - start;

        var result = start + direction * interpValue;

        return result;
    }
}
