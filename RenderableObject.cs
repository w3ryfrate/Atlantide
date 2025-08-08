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

    protected readonly GraphicsHandler Graphics;
    protected abstract float[] Vertices { get; }

    public RenderableObject(GL gl, GraphicsHandler graphics, ShaderProgram program, Matrix4x4 model)
    {
        ShaderProgram = program;
        VAO = new(gl);
        VBO = new(gl, BufferTargetARB.ArrayBuffer);
        Model = model;
        this.gl = gl;
        Graphics = graphics;
    }

    public virtual void Update(double deltaTime)
    {
        if (Disposed)
        {
            Debug.WriteLine("Error::Tried to access a disposed RenderableObject::" + GetType().Name);
            return;
        }
    }

    public virtual void Draw()
    {
        if (Disposed)
        {
            Debug.WriteLine("Error::Tried to access a disposed RenderableObject::" + GetType().Name);
            return;
        }

        ShaderProgram.SetUniformMat4("uModel", Model);
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
