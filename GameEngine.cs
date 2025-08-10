#pragma warning disable CS8618

using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Numerics;
using ProjectNewWorld.Core.GLObjects;
using Shader = ProjectNewWorld.Core.GLObjects.Shader;
using Silk.NET.Input;
using ProjectNewWorld.Core.Cameras;
using ProjectNewWorld.Core.Input;

namespace ProjectNewWorld.Core;

public class GameEngine
{
    public readonly IWindow MainWindow;
    public GraphicsHandler GraphicsHandler { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public BaseCamera Camera { get; private set; }

    private GL _gl;
    private IInputContext _input;

    private Objects.Rectangle _rectangle;
    private Objects.Triangle _triangle;

    ShaderProgram shaderProgram;
    Shader vertShader;
    Shader fragShader;

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
        MainWindow.Center();
        _gl = MainWindow.CreateOpenGL();
        _input = MainWindow.CreateInput();

        InputHandler = new(_input);
        Camera = new FreeViewCamera(this, new(0, 0, -10), Vector3.UnitZ);

        GraphicsHandler = new(this, _gl);
        GraphicsHandler.SetViewport(new(800, 600));

        string vertexCode = File.ReadAllText("Shaders\\plain_vert.shader");
        string fragmentCode = File.ReadAllText("Shaders\\plain_frag.shader");

        vertShader = new(_gl, vertexCode, ShaderType.VertexShader);
        fragShader = new(_gl, fragmentCode, ShaderType.FragmentShader);

        shaderProgram = new(_gl);
        shaderProgram.AttachShader(vertShader);
        shaderProgram.AttachShader(fragShader);
        shaderProgram.Link();

        shaderProgram.DetachShader(vertShader);
        shaderProgram.DetachShader(fragShader);
        vertShader.Dispose();
        fragShader.Dispose();

        _rectangle = new(shaderProgram, this, Vector3.Zero);
        _triangle = new(shaderProgram, this, new(1, 0, 0));
    }

    private void OnUpdate(double delta)
    {
        Camera.Update(delta);
        _rectangle.Update(delta);
        _triangle.Update(delta);
    }

    private void OnRender(double delta)
    {
        GraphicsHandler.Clear(System.Drawing.Color.CornflowerBlue);

        _rectangle.Draw();
        _triangle.Draw();

        MainWindow.SwapBuffers();
    }
}
