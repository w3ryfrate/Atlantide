using Silk.NET.OpenGL;

namespace ProjectNewWorld.Core.GLObjects;

public abstract class DisposableObject : IDisposable
{
    public bool Disposed { get; protected set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
    }

    protected void ThrowIfDisposed<T>(BufferTargetARB? target = null)
    {
        if (target == null)
        {
            ObjectDisposedException.ThrowIf(Disposed, typeof(T));
        }
        else
        {
            ObjectDisposedException.ThrowIf(Disposed, $"{typeof(T)}:{target}");
        }
    }

    protected abstract void Dispose(bool disposing);
}
