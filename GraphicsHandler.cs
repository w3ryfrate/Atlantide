using Silk.NET.OpenGL;
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
        _engine.MainWindow.Size = new(Viewport.Width, Viewport.Height);
        Projection = Matrix4x4.CreatePerspectiveFieldOfView(_engine.Camera.FieldOfView, Viewport.GetAspectRatio(), 0.1f, 100f);
        _gl.Viewport(0, 0, (uint)Viewport.Width, (uint)Viewport.Height);
    }

    public void Clear(System.Drawing.Color color)
    {
        _gl.ClearColor(color);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }
}
