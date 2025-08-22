using Core.Helpers;
using Core.Input;
using Silk.NET.Input;
using System.Numerics;

namespace Core.Cameras;

public class FreeViewCamera : BaseCamera
{
    public readonly float Sensitivity;

    private readonly InputHandler _input;

    private float _yaw = 0f; // Right-Left
    private float _pitch = 0f; // Up-Down
    private float _latestDelta = 0f;

    private Vector3 _initialForward;
    private Vector3 _forward;
    private Vector2 _previousMousePosition = Vector2.Zero;

    public FreeViewCamera(Game engine, Vector3 initialPosition, Vector3 forward, float sensitivity) : base(engine, initialPosition, initialPosition + forward)
    {
        this.FieldOfView = (float)MathHelper.ToRadians(75);
        this.Sensitivity = sensitivity;

        _input = _engine.InputHandler;
        _input.Mouse.MouseMove += OnMouseMove;

        _forward = forward;
        _initialForward = _forward;
    }

    public override void Update(double delta)
    {
        _latestDelta = (float)delta;
        _previousMousePosition = _input.Mouse.Position;
        float trueVelocity = this.Velocity * (float)delta;

        if (_input.Keyboard.IsKeyPressed(Key.W))
        {
            this.Position += this._forward * trueVelocity;
        }
        else if (_input.Keyboard.IsKeyPressed(Key.S))
        {
            this.Position -= this._forward * trueVelocity;
        }

        Vector3 xAxis = Vector3.Cross(_forward, Vector3.UnitY);
        if (_input.Keyboard.IsKeyPressed(Key.A))
        {
            this.Position -= xAxis * trueVelocity;
        }
        else if (_input.Keyboard.IsKeyPressed(Key.D))
        {
            this.Position += xAxis * trueVelocity;
        }

        if (_input.Keyboard.IsKeyPressed(Key.Space))
        {
            this.Position += Vector3.UnitY * trueVelocity;
        }
        else if (_input.Keyboard.IsKeyPressed(Key.AltLeft))
        {
            this.Position -= Vector3.UnitY * trueVelocity;
        }

        if (_input.Keyboard.IsKeyPressed(Key.R))
        {
            this.Reset();
        }

        this.Target = this.Position + _forward; 
        base.Update(delta);
    }

    public override void Reset()
    {
        base.Reset();
        _forward = _initialForward;
        _yaw = 0f;
        _pitch = 0f;
    }

    private void OnMouseMove(IMouse mouse, Vector2 position)
    {
        Vector2 deltaPos = position - _previousMousePosition;
        _yaw += deltaPos.X * this.Sensitivity * _latestDelta;
        _pitch += deltaPos.Y * this.Sensitivity * _latestDelta;
        _pitch = Math.Clamp(_pitch, -89f, 89f);

        float _yawRad = _yaw.ToRadians();
        float _pitchRad = _pitch.ToRadians();
        _forward.X = (float)(-Math.Cos(_pitchRad) * Math.Sin(_yawRad));
        _forward.Y = (float)Math.Sin(_pitchRad);
        _forward.Z = (float)(Math.Cos(_pitchRad) * Math.Cos(_yawRad));
        _forward = -Vector3.Normalize(_forward);
    }
}
