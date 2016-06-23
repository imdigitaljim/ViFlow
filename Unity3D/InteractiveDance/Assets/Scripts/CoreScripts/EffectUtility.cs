using UnityEngine;
using System.Collections;

public static class EffectUtility  {

    public static Color FloatToColor(float x, float y)
    {
        var z = Mathf.Clamp((1 - x + y), 0, 1);
        var r = Mathf.Clamp((2.5623f * x + (-1.1661f) * y + (-.3962f) * z), 0, 1);
        var g = Mathf.Clamp(((-1.0215f) * x + 1.9778f * y + 0.0437f * z), 0, 1);
        var b = Mathf.Clamp((0.0752f * x + (-0.2562f) * y + 1.1810f * z), 0, 1);
        return new Color(r, g, b);
    }
}
