using Silk.NET.OpenGL;

namespace ProjectNewWorld.Core.Objects;

public class Shader : DisposableObject
{
    public uint Handle { get; private set; }
    public ShaderType Type { get; private set; }
    private bool _disposed = false;
    private readonly GL _gl;

    public Shader(GL gl, ShaderType type)
    {
        _gl = gl;
        Handle = _gl.CreateShader(type);
        Type = type;
    }

    public void SetSource(string source)
    {
        ThrowIfDisposed<Shader>();
        _gl.ShaderSource(Handle, source);
    }

    public void Compile()
    {
        ThrowIfDisposed<Shader>();
        _gl.CompileShader(Handle);
        _gl.GetShader(Handle, ShaderParameterName.CompileStatus, out int compStatus);
        if (compStatus == (int)GLEnum.False)
        {
            throw new Exception(_gl.GetShaderInfoLog(Handle));
        }
    }

    public void Delete()
    {
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
