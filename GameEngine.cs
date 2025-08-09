#pragma warning disable CS8618

using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Numerics;
using ProjectNewWorld.Core.Objects;
using Shader = ProjectNewWorld.Core.Objects.Shader;
using Silk.NET.Input;
using ProjectNewWorld.Core.Camera;

namespace ProjectNewWorld.Core;

public class GameEngine
{
    public readonly IWindow MainWindow;
    public GraphicsHandler GraphicsHandler { get; private set; }
    public IInputContext Input { get; private set; }
    public BaseCamera Camera { get; private set; }

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
        MainWindow.Size = new(800, 600);
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
        Input = MainWindow.CreateInput();

        Camera = new FreeViewCamera(Vector3.Zero);
        GraphicsHandler = new(_gl, this);
        GraphicsHandler.SetViewport(new(800, 600));

        string vertexCode = File.ReadAllText("Shaders\\plain_vert.shader");
        string fragmentCode = File.ReadAllText("Shaders\\plain_frag.shader");

        vertShader = new(_gl, vertexCode, ShaderType.VertexShader);
        fragShader = new(_gl,fragmentCode, ShaderType.FragmentShader);

        shaderProgram = new(_gl);
        shaderProgram.AttachShader(vertShader);
        shaderProgram.AttachShader(fragShader);
        shaderProgram.Link();

        shaderProgram.DetachShader(vertShader);
        shaderProgram.DetachShader(fragShader);
        vertShader.Dispose();
        fragShader.Dispose();

        _rectangle = new(_gl, shaderProgram, Matrix4x4.Identity, this);
    }

    private void OnUpdate(double delta)
    {
        Camera.Update(Input.Keyboards[0]);
        _rectangle.Update(delta);
    }

    private void OnRender(double delta)
    {
        GraphicsHandler.Clear(Color.CornflowerBlue);

        _rectangle.Draw(Camera);

        MainWindow.SwapBuffers();
    }
}
