#pragma warning disable CS8618

using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Numerics;
using System.Drawing;
using Shader = Core.Objects.OpenGL.Shader;
using Silk.NET.Input;
using Core.Cameras;
using Core.Input;
using Core.Objects;
using Core.Objects.OpenGL;

namespace Core;

public class Game
{
    public readonly IWindow MainWindow;

    public GL GL { get; private set; }
    public GraphicsHandler Renderer { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public BaseCamera Camera { get; private set; }
    public ConsoleLogger ConsoleLogger { get; private set; }

    private IInputContext _input;

    private RenderableObject[] _renderableObjects = new RenderableObject[3];

    ShaderProgram shaderProgram;
    Shader vertShader;
    Shader fragShader;

    public Game()
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
        ConsoleLogger = new();

        MainWindow.Center();
        GL = MainWindow.CreateOpenGL();
        ConsoleLogger.LogInformation("Created OpenGL");
        _input = MainWindow.CreateInput();
        ConsoleLogger.LogInformation("Created IInputContext");

        InputHandler = new(_input);
        InputHandler.Mouse.Cursor.CursorMode = CursorMode.Raw;

        Camera = new FreeViewCamera(this, new(0, 0, 5), -Vector3.UnitZ, 10f);

        ShaderProgramFactory.CreatePrograms(this);

        Renderer = new(this);
        Renderer.SetViewport(new(1080, 750));

        vertShader = new(this, "Assets\\Shaders\\plain_vert.shader", ShaderType.VertexShader);
        fragShader = new(this, "Assets\\Shaders\\gradient_frag.shader", ShaderType.FragmentShader);

        shaderProgram = new(this, vertShader, fragShader);

        vertShader.Dispose();
        fragShader.Dispose();

        _renderableObjects[0] = new Triangle(this, new(Vector3.Zero));
        _renderableObjects[1] = new Objects.Rectangle(this, new(Vector3.UnitX));
        _renderableObjects[2] = new Voxel(this, new(Vector3.UnitX * 2, Vector3.Zero, 0.5f));
    }

    private void OnUpdate(double delta)
    {
        if (InputHandler.Keyboard.IsKeyPressed(Key.Escape))
        {
            MainWindow.Close();
        }

        Camera.Update(delta);
    }

    private void OnRender(double delta)
    {
        Renderer.Clear(Color.CornflowerBlue);

        foreach (var obj in _renderableObjects)
        {
            if (obj is Voxel)
            {
                Voxel voxel = obj as Voxel;
                Renderer.DrawObject(voxel, shaderProgram);
            }
            else if (obj is Triangle)
            {
                Triangle triangle = obj as Triangle;
                Renderer.DrawObject(triangle, shaderProgram);
            }
            else if (obj is Objects.Rectangle)
            {
                Objects.Rectangle rect = obj as Objects.Rectangle;
                Renderer.DrawObject(rect, shaderProgram);
            }
        }
        
        MainWindow.SwapBuffers();
    }
}
