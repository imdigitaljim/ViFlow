using UnityEngine;
using System.Collections;

public static class EffectUtility  {

    public static Color Vector2ToColor(Vector2 value)
    {
        var z = Mathf.Clamp((1 - value.x + value.y), 0, 1);
        var r = Mathf.Clamp((2.5623f * value.x + (-1.1661f) * value.y + (-.3962f) * z), 0, 1);
        var g = Mathf.Clamp(((-1.0215f) * value.x + 1.9778f * value.y + 0.0437f * z), 0, 1);
        var b = Mathf.Clamp((0.0752f * value.x + (-0.2562f) * value.y + 1.1810f * z), 0, 1);
        return new Color(r, g, b);
    }
}
