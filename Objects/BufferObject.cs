using Silk.NET.OpenGL;

namespace ProjectNewWorld.Core.Objects;

public class BufferObject : DisposableObject
{
    public uint Handle { get; private set; }
    private readonly GL _gl;
    private readonly BufferTargetARB _target;

    public BufferObject(GL gl, BufferTargetARB target)
    {
        _gl = gl;
        _target = target;
        Handle = _gl.GenBuffer();
    }

    public void Bind()
    {
        ThrowIfDisposed<BufferObject>(_target);
        _gl.BindBuffer(_target, Handle);
    }

    public void Unbind()
    {
        ThrowIfDisposed<BufferObject>(_target);
        _gl.BindBuffer(_target, 0);
    }

    public unsafe void SetData(nuint size, void* data, BufferUsageARB usage)
    {
        ThrowIfDisposed<BufferObject>(_target);
        _gl.BufferData(_target, size, data, usage);
    }

    public void Delete()
    {
        _gl.DeleteBuffer(Handle);
        Handle = 0;
    }

    ~BufferObject()
    {
        Dispose(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        if (disposing)
        {

        }

        Delete();
        Disposed = true;
    }
}
