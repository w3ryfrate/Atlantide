using ProjectNewWorld.Core.Helpers;
using ProjectNewWorld.Core.Objects;
using System.Numerics;

namespace ProjectNewWorld.Core.Cameras;

public abstract class BaseCamera
{
    public Vector3 Position { get; protected set; }
    public Vector3 Target { get; protected set; }

    public Matrix4x4 View { get; protected set; }

    public float FieldOfView { get; protected set; } 
    public float Velocity { get; protected set; }

    protected readonly Vector3 _initialPosition;
    protected readonly Vector3 _initialTarget;
    protected readonly GameEngine _engine;

    public BaseCamera(GameEngine engine, Vector3 initialPosition, Vector3 initialTarget)
    {
        _initialPosition = initialPosition;
        _initialTarget = initialTarget;
        _engine = engine;
        Position = initialPosition;
        Target = initialTarget;
        FieldOfView = (float)MathHelper.ToRadians(80d);
        Velocity = 10f;
    }

    public virtual void Update(double delta)
    {
        View = Matrix4x4.CreateLookAt(Position, Target, Vector3.UnitY);
    }

    /// <summary>
    /// Resets the camera to its initial properties.
    /// </summary>
    public virtual void Reset()
    {
        this.Position = _initialPosition;
        this.Target = _initialTarget;
    }
}
