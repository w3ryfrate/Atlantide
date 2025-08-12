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

    private readonly GameEngine _engine;
    private readonly GL _gl;

    public GraphicsHandler(GameEngine engine)
    {
        _engine = engine;
        _gl = _engine.GL;
    }

    public void SetViewport(Viewport viewport)
    {
        Viewport = viewport;
        _engine.MainWindow.Size = new(this.Viewport.Size.Width, this.Viewport.Size.Height);
        Projection = Matrix4x4.CreatePerspectiveFieldOfView(_engine.Camera.FieldOfView, Viewport.GetAspectRatio(), 0.1f, 100f);
        _gl.Viewport(0, 0, (uint)Viewport.Size.Width, (uint)Viewport.Size.Height);
    }

    public void Clear(System.Drawing.Color color)
    {
        _gl.ClearColor(color);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    private void PrepareDrawing(RenderableObject obj, ShaderProgram shaderProgram, Color color)
    {
        obj.BeforeDraw.Invoke(obj, new());
        //obj.ShaderProgram.SetUniform2("uViewportSize", this.Viewport.Size.ToVector2());
        shaderProgram.SetUniform4("uColor1", color.ToVector4());
        shaderProgram.SetUniform4("uColor2", color.Invert().ToVector4());
        shaderProgram.SetUniformMat4("uMVP", obj.Model * this._engine.Camera.View * this.Projection);
        shaderProgram.Use();
        obj.VAO.Bind();
    }

    // TODO: Add ShaderProgram as an argument here and remove it from RenderableObject
    public void DrawRectangle(Objects.Rectangle rect, ShaderProgram shaderProgram, Color color)
    {
        if (rect.Disposed)
            return;

        PrepareDrawing(rect, shaderProgram, color);
        unsafe
        {
            _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0);
        }
    }

    public void DrawTriangle(Triangle triangle, ShaderProgram shaderProgram, Color color)
    {
        if (triangle.Disposed)
            return;

        PrepareDrawing(triangle, shaderProgram, color);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }
}
