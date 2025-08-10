using System.Numerics;

namespace ProjectNewWorld.Core.Objects;

public interface ITransformableObject
{
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }
    public float Scale { get; set; }
}
