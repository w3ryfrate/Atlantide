using ProjectNewWorld.Core.Helpers;
using ProjectNewWorld.Core.Input;
using Silk.NET.Input;
using System.Diagnostics;
using System.Numerics;

namespace ProjectNewWorld.Core.Cameras;

public class FreeViewCamera : BaseCamera
{
    private readonly InputHandler _input;

    private float _yaw = 0f; // Right-Left
    private float _pitch = 0f; // Up-Down

    public FreeViewCamera(GameEngine engine, Vector3 initialPosition, Vector3 initialTarget) : base(engine, initialPosition, initialTarget)
    {
        this.FieldOfView = (float)MathHelper.ToRadians(75);

        _input = _engine.InputHandler;
        _input.Mouse.MouseMove += OnMouseMove;
    }

    public override void Update(double delta)
    {
        Vector3 directionVector = Vector3.Zero;
        if (_input.Keyboard.IsKeyPressed(Key.W))
        {
            directionVector.Z = 1;
        }
        else if (_input.Keyboard.IsKeyPressed(Key.S))
        {
            directionVector.Z = -1;
        }

        if (_input.Keyboard.IsKeyPressed(Key.A))
        {
            directionVector.X = 1;
        }
        else if (_input.Keyboard.IsKeyPressed(Key.D))
        {
            directionVector.X = -1;
        }

        if (_input.Keyboard.IsKeyPressed(Key.Q))
        {
            directionVector.Y = 1;
        }
        else if (_input.Keyboard.IsKeyPressed(Key.E))
        {
            directionVector.Y = -1;
        }

        if (_input.Keyboard.IsKeyPressed(Key.R))
        {
            this.Position = this._initialPosition;
            this.Target = this._initialTarget;
            directionVector = Vector3.Zero;
        }

        if (directionVector != Vector3.Zero)
            Vector3.Normalize(directionVector);

        this.Position += directionVector * (float)delta * this.Velocity;
        //this.Target = _yaw + _pitch;
        base.Update(delta);
    }

    private void OnMouseMove(IMouse mouse, Vector2 oldPosition)
    {
        float deltaX = mouse.Position.X - oldPosition.X;
        float deltaY = mouse.Position.Y - oldPosition.Y;
        _yaw += deltaX;
        _pitch += deltaY;
        if (deltaX > 0 && deltaY > 0)
            Debug.WriteLine($"Yaw: {_yaw}", $"Pitch: {_pitch}");
    }
}
