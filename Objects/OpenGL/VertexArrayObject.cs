using ProjectNewWorld.Core.Objects;
using Silk.NET.OpenGL;

namespace ProjectNewWorld.Core.Objects.OpenGL;

public class VertexArrayObject : DisposableObject
{
    public uint Handle { get; private set; }
    private readonly GL _gl;

    public VertexArrayObject(GL gl)
    {
        _gl = gl;
        Handle = _gl.GenVertexArray();
    }

    public void Bind()
    {
        ThrowIfDisposed<VertexArrayObject>();
        _gl.BindVertexArray(Handle);
    }

    public void Unbind()
    {
        ThrowIfDisposed<VertexArrayObject>();
        _gl.BindVertexArray(0);
    }

    public void Delete()
    {
        ThrowIfDisposed<VertexArrayObject>();
        _gl.DeleteVertexArray(Handle);
        Handle = 0;
    }

    ~VertexArrayObject()
    {
        Dispose(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        Delete();
        Disposed = true;
    }
}
