
using Silk.NET.OpenGL;

namespace Core.Objects.OpenGL;

public class Shader : DisposableObject
{
    public uint Handle { get; private set; }
    public ShaderType Type { get; private set; }

    private readonly string _path;
    private readonly Game _game;
    private readonly GL _gl;

    public Shader(Game game, string file, ShaderType type)
    {
        _game = game;
        _gl = game.GL;
        _path = file;
        Handle = _gl.CreateShader(type);
        Type = type;
        _gl.ShaderSource(Handle, File.ReadAllText(file));
        this.Compile();
    }

    private void Compile()
    {
        _gl.CompileShader(Handle);
        _gl.GetShader(Handle, ShaderParameterName.CompileStatus, out int compStatus);
        if (compStatus == (int)GLEnum.False)
        {
            _game.ConsoleLogger.LogFatal($"Shader:{_path} compilation error: {_gl.GetShaderInfoLog(Handle)}");
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
        if (Disposed)
            return;

        if (disposing)
        {
            
        }

        Delete();
        Disposed = true;
    }
}
