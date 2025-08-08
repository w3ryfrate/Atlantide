using Silk.NET.OpenGL;
using System.Numerics;
namespace ProjectNewWorld.Core;

public class GraphicsHandler
{
    public Matrix4x4 Projection;
    public Matrix4x4 View;
    public Viewport Viewport { get; private set; }

    private readonly GL _gl;
    private readonly GameEngine _engine;

    public GraphicsHandler(GL gl, GameEngine engine)
    {
        _gl = gl;
        _engine = engine;
    }

    public void SetViewport(Viewport viewport)
    {
        Viewport = viewport;
        _engine.MainWindow.Size = new(Viewport.Width, Viewport.Height);
        _gl.Viewport(0, 0, (uint)Viewport.Width, (uint)Viewport.Height);
    }

    public void Clear(System.Drawing.Color color)
    {
        _gl.ClearColor(color);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }
}
