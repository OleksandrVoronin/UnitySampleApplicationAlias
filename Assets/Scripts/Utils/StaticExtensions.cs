using UnityEngine;

/// <summary>
/// Static extensions.
/// </summary>
public static class StaticExtensions
{
    public static Color WithAlpha(this Color color, float alpha) {
        return new Color(color.r, color.g, color.b, alpha);
    }
}
