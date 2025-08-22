using Core;
using Core.Objects.OpenGL;
using Core.Helpers;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Core.Objects.OpenGL;

public class ShaderProgram : DisposableObject
{
    public uint Handle { get; private set; }
    public readonly Shader VertexShader;
    public readonly Shader FragmentShader;

    private static readonly Dictionary<ShaderProgramType, ShaderProgram> _shaderPrograms = new();

    private readonly Game _game;
    private readonly GL _gl;

    public ShaderProgram(Game game, Shader vertexShader, Shader fragmentShader)
    {
        _gl = game.GL;
        _game = game;
        Handle = _gl.CreateProgram();

        this.VertexShader = vertexShader;
        this.FragmentShader = fragmentShader;

        if (VertexShader.Disposed) game.ConsoleLogger.LogFatal("Cannot use a disposed vertex shader!");
        else if (VertexShader.Disposed) game.ConsoleLogger.LogFatal("Cannot use a disposed fragment shader!");

        _gl.AttachShader(Handle, VertexShader.Handle);
        _gl.AttachShader(Handle, FragmentShader.Handle);
        _gl.LinkProgram(Handle);

        _gl.DetachShader(Handle, VertexShader.Handle);
        _gl.DetachShader(Handle, FragmentShader.Handle);
    }

    public void Use()
    {
        ThrowIfDisposed<ShaderProgram>();
        _gl.UseProgram(Handle);
    }

    public void SetUniform1(string name, uint value)
    {
        ThrowIfDisposed<ShaderProgram>();
        int loc = this.GetUniformLocation(name);
        if (loc == -1)
            throw new ArgumentException("Uniform not found!");

        _gl.Uniform1(loc, value);
    }
    public void SetUniform1(string name, int value)
    {
        ThrowIfDisposed<ShaderProgram>();
        int loc = this.GetUniformLocation(name);
        if (loc == -1)
            throw new ArgumentException("Uniform not found!");

        _gl.Uniform1(loc, value);
    }
    public void SetUniform1(string name, float value)
    {
        ThrowIfDisposed<ShaderProgram>();
        int loc = this.GetUniformLocation(name);
        if (loc == -1)
            throw new ArgumentException("Error::Uniform not found!::" + name);

        _gl.Uniform1(loc, value);
    }

    public void SetUniform2(string name, Vector2 value)
    {
        ThrowIfDisposed<ShaderProgram>();
        int loc = this.GetUniformLocation(name);
        if (loc == -1)
            throw new ArgumentException("Error::Uniform not found!::" + name);

        _gl.Uniform2(loc, value);
    }

    public void SetUniform4(string name, Vector4 value)
    {
        ThrowIfDisposed<ShaderProgram>();
        int loc = this.GetUniformLocation(name);
        if (loc == -1)
            throw new ArgumentException("Error::Uniform not found!::" + name);

        _gl.Uniform4(loc, value);
    }

    public void SetUniformMat4(string name, Matrix4x4 value)
    {
        ThrowIfDisposed<ShaderProgram>();
        int loc = this.GetUniformLocation(name);
        if (loc == -1)
            throw new ArgumentException("Error::Uniform not found!::" + name);

        _gl.UniformMatrix4(loc, true, value.ToArray());
    }

    public int GetUniformLocation(string name)
    {
        return _gl.GetUniformLocation(Handle, name);
    }

    public void Delete()
    {
        _gl.DeleteProgram(Handle);
    }

    ~ShaderProgram()
    {
        Dispose(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        Handle = 0;
        Delete();
        Disposed = true;
    }
}
