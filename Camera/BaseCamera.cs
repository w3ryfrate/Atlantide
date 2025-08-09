using ProjectNewWorld.Core.Helpers;
using Silk.NET.Input;
using System.Numerics;

namespace ProjectNewWorld.Core.Camera;

public abstract class BaseCamera
{
    public Vector3 Position { get; protected set; }
    public Vector3 Target { get; protected set; }

    public float FieldOfView { get; protected set; } 
    public Matrix4x4 View { get; protected set; }

    public BaseCamera(Vector3 initialPosition)
    {
        Position = initialPosition;
        FieldOfView = (float)MathHelper.ToRadians(80d);
    }

    public virtual void Update(IKeyboard keyboard)
    {
        View = Matrix4x4.CreateLookAt(Position, Target, Vector3.UnitY);
    }
}
