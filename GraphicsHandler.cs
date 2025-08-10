using Silk.NET.OpenGL;
using System.Numerics;
namespace ProjectNewWorld.Core;

public class GraphicsHandler
{
    public Matrix4x4 Projection { get; private set; }
    public Viewport Viewport { get; private set; }
    public readonly GL GL;

    private readonly GameEngine _engine;

    public GraphicsHandler(GameEngine engine, GL gl)
    {
        GL = gl;
        _engine = engine;
    }

    public void SetViewport(Viewport viewport)
    {
        Viewport = viewport;
        _engine.MainWindow.Size = new(Viewport.Width, Viewport.Height);
        Projection = Matrix4x4.CreatePerspectiveFieldOfView(_engine.Camera.FieldOfView, Viewport.GetAspectRatio(), 0.1f, 100f);
        GL.Viewport(0, 0, (uint)Viewport.Width, (uint)Viewport.Height);
    }

    public void Clear(System.Drawing.Color color)
    {
        GL.ClearColor(color);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }
}
