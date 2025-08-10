using ProjectNewWorld.Core.Helpers;
using Silk.NET.OpenGL;
using System.Numerics;

namespace ProjectNewWorld.Core.GLObjects;

public class ShaderProgram : DisposableObject
{
    public uint Handle { get; private set; }
    private (bool vertex, bool frag) _shaders;
    private readonly GL _gl;

    public ShaderProgram(GL gl)
    {
        _gl = gl;
        Handle = _gl.CreateProgram();
        _shaders = (false, false);
    }

    public void Use()
    {
        ThrowIfDisposed<ShaderProgram>();
        _gl.UseProgram(Handle);
    }

    public void AttachShader(Shader shader)
    {
        ThrowIfDisposed<ShaderProgram>();
        _gl.AttachShader(Handle, shader.Handle);

        if (shader.Type == ShaderType.VertexShader)
            _shaders.vertex = true;
        else if (shader.Type == ShaderType.FragmentShader)
            _shaders.frag = true;
    }

    public void DetachShader(Shader shader)
    {
        ThrowIfDisposed<ShaderProgram>();
        _gl.DetachShader(Handle, shader.Handle);

        if (shader.Type == ShaderType.VertexShader)
            _shaders.vertex = false;
        else if (shader.Type == ShaderType.FragmentShader)
            _shaders.frag = false;
    }

    public void Link()
    {
        ThrowIfDisposed<ShaderProgram>();
        if (!_shaders.vertex) throw new InvalidOperationException("Vertex shader not present at link!");
        if (!_shaders.frag) throw new InvalidOperationException("Frag shader not present at link!");
        _gl.LinkProgram(Handle);
    }

    public void SetUniform1(string name, uint value)
    {
        ThrowIfDisposed<ShaderProgram>();
        int loc = _gl.GetUniformLocation(Handle, name);
        if (loc == -1)
            throw new ArgumentException("Uniform not found!");

        _gl.Uniform1(loc, value);
    }
    public void SetUniform1(string name, int value)
    {
        ThrowIfDisposed<ShaderProgram>();
        int loc = _gl.GetUniformLocation(Handle, name);
        if (loc == -1)
            throw new ArgumentException("Uniform not found!");

        _gl.Uniform1(loc, value);
    }
    public void SetUniform1(string name, float value)
    {
        ThrowIfDisposed<ShaderProgram>();
        int loc = _gl.GetUniformLocation(Handle, name);
        if (loc == -1)
            throw new ArgumentException("Error::Uniform not found!::" + name);

        _gl.Uniform1(loc, value);
    }

    public void SetUniformMat4(string name, Matrix4x4 value)
    {
        ThrowIfDisposed<ShaderProgram>();
        int loc = _gl.GetUniformLocation(Handle, name);
        if (loc == -1)
            throw new ArgumentException("Error::Uniform not found!::" + name);

        _gl.UniformMatrix4(loc, true, value.FormatAsArray());
    }

    public void Delete()
    {
        _gl.DeleteProgram(Handle);
        Handle = 0;
        _shaders.vertex = false;
        _shaders.frag = false;
    }

    ~ShaderProgram()
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
