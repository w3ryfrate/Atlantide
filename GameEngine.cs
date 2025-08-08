#pragma warning disable CS8618

using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Numerics;
using StbImageSharp;
using ProjectNewWorld.Core.Objects;
using Shader = ProjectNewWorld.Core.Objects.Shader;
using System.Diagnostics;

namespace ProjectNewWorld.Core;

public class GameEngine
{
    public readonly IWindow MainWindow;
    public GraphicsHandler GraphicsHandler { get; private set; }
    private GL _gl;

    private Rectangle _rectangle;

    ShaderProgram shaderProgram;
    Shader vertShader;
    Shader fragShader;

    private static uint _texture;

    public GameEngine()
    {
        MainWindow = Window.Create(WindowOptions.Default);
        MainWindow.VSync = true;
        MainWindow.WindowBorder = WindowBorder.Fixed;
        MainWindow.ShouldSwapAutomatically = false;

        MainWindow.Load += OnLoad;
        MainWindow.Update += OnUpdate;
        MainWindow.Render += OnRender;
        MainWindow.Run();
    }

    private unsafe void OnLoad()
    {
        _gl = MainWindow.CreateOpenGL();
        GraphicsHandler = new(_gl, this);
        GraphicsHandler.SetViewport(new(800, 600));
        //_graphicsHandler.View = Matrix4x4.CreateLookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);
        //_graphicsHandler.Projection = Matrix4x4.CreatePerspectiveFieldOfView(1.3f, _graphicsHandler.Viewport.GetAspectRatio(), 0.1f, 100f);

        string vertexCode = File.ReadAllText("Shaders\\plain_vert.shader");
        string fragmentCode = File.ReadAllText("Shaders\\plain_frag.shader");

        vertShader = new(_gl, ShaderType.VertexShader);
        vertShader.SetSource(vertexCode);
        vertShader.Compile();

        fragShader = new(_gl, ShaderType.FragmentShader);
        fragShader.SetSource(fragmentCode);
        fragShader.Compile();

        shaderProgram = new(_gl);
        shaderProgram.AttachShader(vertShader);
        shaderProgram.AttachShader(fragShader);
        shaderProgram.Link();

        shaderProgram.DetachShader(vertShader);
        shaderProgram.DetachShader(fragShader);
        vertShader.Dispose();
        fragShader.Dispose();

        _rectangle = new(_gl, shaderProgram, Matrix4x4.Identity, GraphicsHandler);
    }

    private void OnUpdate(double delta)
    {
        if (!_rectangle.Disposed)
            _rectangle.Update(delta);
    }

    private unsafe void OnRender(double delta)
    {
        GraphicsHandler.Clear(Color.CornflowerBlue);

        if (!_rectangle.Disposed) 
            _rectangle.Draw();

        MainWindow.SwapBuffers();
    }
}
