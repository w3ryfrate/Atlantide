using Core.Objects.OpenGL;
using ProjectNewWorld.Core.Objects.OpenGL;
using Silk.NET.OpenGL;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProjectNewWorld.Core.Objects;

public abstract class RenderableObject : DisposableObject
{
    public Matrix4x4 Model => this.Transform.GetModel();

    public readonly Transform Transform;

    public readonly VertexArrayObject VAO;
    public readonly BufferObject VBO;

    protected readonly GL gl;
    protected readonly Game Game;
    protected abstract float[] VertexBufferData { get; }

    public RenderableObject(Game game, Transform transform)
    {
        this.Game = game;
        this.gl = game.GL;

        this.VAO = new(gl);
        this.VBO = new(gl, BufferTargetARB.ArrayBuffer);

        this.Transform = transform;
    }

    public virtual void Update(double delta)
    {
        ThrowIfDisposed<RenderableObject>();
    }

    ~RenderableObject()
    {
        Dispose(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        VAO.Dispose();
        VBO.Dispose();
        Disposed = true;
    }
}
