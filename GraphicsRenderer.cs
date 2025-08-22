using Core.Objects.OpenGL;
using ProjectNewWorld.Core.Helpers;
using ProjectNewWorld.Core.Objects;
using ProjectNewWorld.Core.Objects.OpenGL;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Numerics;

namespace ProjectNewWorld.Core;

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

    public void DrawRectangle(Objects.Rectangle rect, ShaderProgram shaderProgram)
    {
        if (rect.Disposed)
            return;

        _toUpdate.Add(rect);
        PrepareDrawing(rect, shaderProgram);
        unsafe
        {
            _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0);
        }
    }

    public void DrawTriangle(Triangle triangle, ShaderProgram shaderProgram)
    {
        if (triangle.Disposed)
            return;

        _toUpdate.Add(triangle);
        PrepareDrawing(triangle, shaderProgram);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    private void PrepareDrawing(RenderableObject obj, ShaderProgram shaderProgram)
    {
        shaderProgram.SetUniformMat4("uMVP", obj.Model * _game.Camera.View * Projection);
        shaderProgram.SetUniform4("uColor1", Color.Red.ToVector4());
        shaderProgram.SetUniform4("uColor2", Color.Blue.ToVector4());
        shaderProgram.Use();
        obj.VAO.Bind();
    }
}
