using Silk.NET.OpenGL;

namespace ProjectNewWorld.Core.GLObjects;

public class Shader : DisposableObject
{
    public uint Handle { get; private set; }
    public ShaderType Type { get; private set; }
    private bool _disposed = false;
    private readonly GL _gl;

    public Shader(GL gl, string source, ShaderType type)
    {
        _gl = gl;
        Handle = _gl.CreateShader(type);
        Type = type;
        _gl.ShaderSource(Handle, source);
        this.Compile();
    }

    private void Compile()
    {
        _gl.CompileShader(Handle);
        _gl.GetShader(Handle, ShaderParameterName.CompileStatus, out int compStatus);
        if (compStatus == (int)GLEnum.False)
        {
            throw new Exception(_gl.GetShaderInfoLog(Handle));
        }
    }

    public void Delete()
    {
        ThrowIfDisposed<Shader>();
        if (Handle != 0)
        {
            _gl.DeleteShader(Handle);
            Handle = 0;
        }
    }

    ~Shader()
    {
        Dispose(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            
        }

        Delete();
        _disposed = true;
    }
}
