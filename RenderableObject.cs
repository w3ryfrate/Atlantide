using ProjectNewWorld.Core.Camera;
using ProjectNewWorld.Core.Objects;
using Silk.NET.OpenGL;
using System.Diagnostics;
using System.Numerics;

namespace ProjectNewWorld.Core;

public abstract class RenderableObject : DisposableObject
{
    protected readonly GL gl;

    public Matrix4x4 Model { get; protected set; }
    public readonly ShaderProgram ShaderProgram;
    public readonly VertexArrayObject VAO;
    public readonly BufferObject VBO;

    protected readonly GameEngine Engine;
    protected abstract float[] Vertices { get; }

    public RenderableObject(GL gl, GameEngine engine, ShaderProgram program, Matrix4x4 model)
    {
        ShaderProgram = program;
        VAO = new(gl);
        VBO = new(gl, BufferTargetARB.ArrayBuffer);
        Model = model;
        this.gl = gl;
        Engine = engine;
    }

    public virtual void Update(double deltaTime)
    {
        ThrowIfDisposed<RenderableObject>();
    }

    public virtual void Draw(BaseCamera camera)
    {
        ThrowIfDisposed<RenderableObject>();
        ShaderProgram.SetUniformMat4("uProjection", Engine.GraphicsHandler.Projection);
        ShaderProgram.SetUniformMat4("uView", camera.View);
        ShaderProgram.SetUniformMat4("uModel", Model);
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
            Model = Matrix4x4.Identity;
        }

        VAO.Dispose();
        VBO.Dispose();
        Disposed = true;
    }
}
