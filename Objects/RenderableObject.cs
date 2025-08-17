using ProjectNewWorld.Core.Objects.OpenGL;
using Silk.NET.OpenGL;
using System.Numerics;

namespace ProjectNewWorld.Core.Objects;

public abstract class RenderableObject : DisposableObject
{
    public readonly Transform Transform;

    public readonly VertexArrayObject VAO;
    public readonly BufferObject VBO;

    /// <summary>
    /// Fires just before this object is drawn.
    /// </summary>
    public readonly EventHandler<EventArgs> BeforeDraw;

    protected readonly GL gl;
    protected readonly Game Engine;
    protected abstract float[] VertexBufferData { get; }

    public Matrix4x4 Model => this.Transform.GetModel();

    public RenderableObject(Game engine, Transform transform)
    {
        this.Engine = engine;
        this.gl = engine.GL;

        this.VAO = new(gl);
        this.VBO = new(gl, BufferTargetARB.ArrayBuffer);

        this.Transform = transform;

        this.BeforeDraw += OnBeforeDraw;
    }

    public virtual void Update(double deltaTime)
    {
        ThrowIfDisposed<RenderableObject>();
    }

    protected virtual void OnBeforeDraw(object? sender, EventArgs e)
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
