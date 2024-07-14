using Godot;

public class SineInterpolator
{





    /// <summary>
    /// Custom 1D 1/2 Sine Interpolation.  Slow to start and end.
    /// </summary>
    public static float HalfInterpolate(float start, float end, float cursor)
    {
        // y =  -0.5 * cos(x * pi) + 0.5

        var interpValue = -0.5f * Mathf.Cos(Mathf.Pi * cursor) + 0.5f;
        var direction = end - start;

        var result = start + direction * interpValue;

        return result;
    }



    /// <summary>
    /// Custom 3D 1/2 Sine Interpolation.  Slow to start and end.
    /// </summary>
    public static Vector3 HalfInterpolate(Vector3 start, Vector3 end, float cursor)
    {
        // y =  -0.5 * cos(x * pi) + 0.5

        var interpValue = -0.5f * Mathf.Cos(Mathf.Pi * cursor) + 0.5f;
        var direction = end - start;

        var result = start + direction * interpValue;

        return result;
    }



    /// <summary>
    /// Custom 1D 1/4 Sine Interpolation.  Fast to start and slow to end.
    /// </summary>
    public static float QuarterInterpolate(float start, float end, float cursor)
    {
        // y =  sin( x * (pi / 2))

        var interpValue = Mathf.Sin(Mathf.Pi * 0.5f * cursor);
        var direction = end - start;

        var result = start + direction * interpValue;

        return result;
    }



    /// <summary>
    /// Custom 3D 1/4 Sine Interpolation.  Fast to start and slow to end.
    /// </summary>
    public static Vector3 QuarterInterpolate(Vector3 start, Vector3 end, float cursor)
    {
        // y =  sin( x * (pi / 2))

        var interpValue = Mathf.Sin(Mathf.Pi * 0.5f * cursor);
        var direction = end - start;

        var result = start + direction * interpValue;

        return result;
    }
}
