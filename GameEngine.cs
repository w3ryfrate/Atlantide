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

    public GL GL { get; private set; }
    public GraphicsHandler GraphicsHandler { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public BaseCamera Camera { get; private set; }

    private IInputContext _input;

    private Objects.Rectangle _rectangle;
    private Objects.Triangle _triangle;

    ShaderProgram shaderProgram;
    Shader vertShader;
    Shader fragShader;

    public GameEngine()
    {
        MainWindow = Window.Create(WindowOptions.Default);
        MainWindow.FramesPerSecond = 75;
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
        GL = MainWindow.CreateOpenGL();
        _input = MainWindow.CreateInput();

        InputHandler = new(_input);
        InputHandler.Mouse.Cursor.CursorMode = CursorMode.Raw;

        Camera = new FreeViewCamera(this, new(0, 0, -10), Vector3.UnitZ, 10f);

        GraphicsHandler = new(this);
        GraphicsHandler.SetViewport(new(1080, 750));

        vertShader = new(GL, "Shaders\\plain_vert.shader", ShaderType.VertexShader);
        fragShader = new(GL, "Shaders\\plain_frag.shader", ShaderType.FragmentShader);

        shaderProgram = new(GL);
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

        if (InputHandler.Keyboard.IsKeyPressed(Key.Escape))
        {
            MainWindow.Close();
        }
    }

    private void OnRender(double delta)
    {
        GraphicsHandler.Clear(System.Drawing.Color.CornflowerBlue);

        _rectangle.Draw();
        _triangle.Draw();

        MainWindow.SwapBuffers();
    }
}
