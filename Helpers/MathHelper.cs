using System.Numerics;

namespace ProjectNewWorld.Core.Helpers;

public static class MathHelper
{
    public static float[] FormatAsArray(this Matrix4x4 matrix)
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

    public static double ToDegrees(this double radians) => radians * 180.0d / Math.PI;
    public static double ToRadians(this double degrees) => degrees * Math.PI / 180.0d;
}
