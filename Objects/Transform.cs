using System.Numerics;

namespace ProjectNewWorld.Core.Objects;

public class Transform
{
    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            _model.Translation = value;
        }
    }
    private Vector3 _position;

    public Vector3 Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;

            _rotationXMatrix = Matrix4x4.CreateRotationX(value.X);
            _rotationYMatrix = Matrix4x4.CreateRotationY(value.Y);
            _rotationZMatrix = Matrix4x4.CreateRotationZ(value.Z);

            this.UpdateModel();
        }
    }
    private Vector3 _rotation;

    public float Scale
    {
        get => _scale;
        set
        {
            _scale = value;

            _scaleMatrix = Matrix4x4.CreateScale(value);

            this.UpdateModel();
        }
    }
    private float _scale;

    private Matrix4x4 _model;
    private Matrix4x4 RotationMatrix => _rotationXMatrix * _rotationYMatrix * _rotationZMatrix;
    private Matrix4x4 _rotationXMatrix = Matrix4x4.Identity;
    private Matrix4x4 _rotationYMatrix = Matrix4x4.Identity;
    private Matrix4x4 _rotationZMatrix = Matrix4x4.Identity;
    private Matrix4x4 _scaleMatrix = Matrix4x4.Identity;

    public Transform(Vector3 position)
    {
        Position = position;
        Rotation = Vector3.Zero;
        Scale = 1f;
    }

    public Matrix4x4 GetModel()
    {
        this.UpdateModel();
        return _model;
    }

    private void UpdateModel()
    {
        _model = _scaleMatrix * this.RotationMatrix;
        _model.Translation = this.Position;
    }
}
