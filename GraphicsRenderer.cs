using Core.Objects.OpenGL;
using Core.Helpers;
using Core.Objects;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Numerics;

namespace Core;

public class GraphicsHandler
{
    public Matrix4x4 Projection { get; private set; }
    public Viewport Viewport { get; private set; }

    private List<RenderableObject> _toUpdate = new();
    private readonly Game _game;
    private readonly GL _gl;

    public GraphicsHandler(Game game)
    {
        _game = game;
        _gl = _game.GL;
    }

    public void SetViewport(Viewport viewport)
    {
        Viewport = viewport;
        _game.MainWindow.Size = new(this.Viewport.Size.Width, this.Viewport.Size.Height);
        Projection = Matrix4x4.CreatePerspectiveFieldOfView(_game.Camera.FieldOfView, Viewport.GetAspectRatio(), 0.1f, 100f);
        _gl.Viewport(0, 0, (uint)Viewport.Size.Width, (uint)Viewport.Size.Height);
    }

    public void Clear(Color color)
    {
        _gl.ClearColor(color);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void Update(double delta)
    {
        foreach (var obj in _toUpdate)
        {
            if (!obj.Disposed)
                obj.Update(delta);
        }

        _toUpdate.Clear();
    }

    public void DrawObject(Objects.Rectangle rect, ShaderProgram shaderProgram)
    {
        if (rect.Disposed)
            return;

        _toUpdate.Add(rect);
        UseShaderProgram(rect, shaderProgram);
        unsafe
        {
            _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0);
        }
    }
    public void DrawObject(Triangle triangle, ShaderProgram shaderProgram)
    {
        if (triangle.Disposed)
            return;

        _toUpdate.Add(triangle);
        UseShaderProgram(triangle, shaderProgram);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }
    public void DrawObject(Voxel voxel, ShaderProgram shaderProgram)
    {
        if (voxel.Disposed)
            return;

        _toUpdate.Add(voxel);
        UseShaderProgram(voxel, shaderProgram);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }

    private void UseShaderProgram(RenderableObject obj, ShaderProgram shaderProgram)
    {
        shaderProgram.SetUniformMat4("uMVP", obj.Model * _game.Camera.View * Projection);
        shaderProgram.SetUniform4("uColor1", Color.Red.ToVector4());
        shaderProgram.SetUniform4("uColor2", Color.Blue.ToVector4());
        shaderProgram.Use();
        obj.VAO.Bind();
    }
}
