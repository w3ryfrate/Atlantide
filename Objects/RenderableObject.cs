using ProjectNewWorld.Core.Cameras;
using ProjectNewWorld.Core.GLObjects;
using Silk.NET.OpenGL;
using System.Diagnostics;
using System.Numerics;

namespace ProjectNewWorld.Core.Objects;

public abstract class RenderableObject : DisposableObject, ITransformableObject
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

            Matrix4x4 rotationMatrix = _rotationXMatrix * _rotationYMatrix * _rotationZMatrix;
            _model = _scaleMatrix * rotationMatrix;
            _model.Translation = _position;
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
            Matrix4x4 _rotationMatrix = _rotationXMatrix * _rotationYMatrix * _rotationZMatrix;
            _model = _scaleMatrix * _rotationMatrix;
            _model.Translation = _position;
        }
    }
    private float _scale;

    public readonly ShaderProgram ShaderProgram;
    public readonly VertexArrayObject VAO;
    public readonly BufferObject VBO;

    private Matrix4x4 _rotationXMatrix = Matrix4x4.Identity;
    private Matrix4x4 _rotationYMatrix = Matrix4x4.Identity;
    private Matrix4x4 _rotationZMatrix = Matrix4x4.Identity; 
    private Matrix4x4 _scaleMatrix = Matrix4x4.Identity;

    protected readonly GL gl;
    protected readonly GameEngine Engine;
    protected abstract float[] Vertices { get; }

    protected Matrix4x4 Model => _model;
    private Matrix4x4 _model = Matrix4x4.Identity;

    public RenderableObject(ShaderProgram shaderProgram, GameEngine engine, Vector3 position)
    {
        this.Engine = engine;
        this.gl = engine.GraphicsHandler.GL;

        this.VAO = new(gl);
        this.VBO = new(gl, BufferTargetARB.ArrayBuffer);
        this.ShaderProgram = shaderProgram;

        this.Position = position;
        this.Rotation = Vector3.Zero;
        this.Scale = 1f;
    }

    float time = 0f;
    public virtual void Update(double deltaTime)
    {
        ThrowIfDisposed<RenderableObject>();
        time += (float)deltaTime;
        ShaderProgram.SetUniform1("uTime", time);
    }

    public virtual void Draw()
    {
        ThrowIfDisposed<RenderableObject>();
        ShaderProgram.SetUniformMat4("uProjection", Engine.GraphicsHandler.Projection);
        ShaderProgram.SetUniformMat4("uView", Engine.Camera.View);
        ShaderProgram.SetUniformMat4("uModel", _model);
        ShaderProgram.Use();
        VAO.Bind();
    }

    protected override void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        if (disposing)
        {
            Array.Clear(Vertices);
            _model = Matrix4x4.Identity;
        }

        VAO.Dispose();
        VBO.Dispose();
        Disposed = true;
    }
}
