using System.Drawing;
using System.Numerics;

namespace ProjectNewWorld.Core.Helpers;

public static class MathHelper
{
    public static float[] ToArray(this Matrix4x4 matrix)
    {
        float[] array =
        {
            matrix.M11, matrix.M12, matrix.M13, matrix.M14,
            matrix.M21, matrix.M22, matrix.M23, matrix.M24,
            matrix.M31, matrix.M32, matrix.M33, matrix.M34,
            matrix.M41, matrix.M42, matrix.M43, matrix.M44,
        };

        return array;
    }

    public static Vector2 ToVector2(this Size size)
    {
        Vector2 vector = new(size.Width, size.Height);
        return vector;
    }
    public static Vector4 ToVector4(this Color color)
    {
        Vector4 vector = new(color.R / 255f, color.G / 255f, color.B / 255f, 1f);
        return vector;
    }
    public static Color Invert(this Color color)
    {
        return Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
    }

    public static double ToDegrees(this double radians) => radians * 180.0d / Math.PI;
    public static double ToRadians(this double degrees) => degrees * Math.PI / 180.0d;

    public static float ToDegrees(this float radians) => radians * 180.0f / (float)Math.PI;
    public static float ToRadians(this float degrees) => degrees * (float)Math.PI / 180.0f;
}
