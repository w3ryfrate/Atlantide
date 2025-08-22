#pragma warning disable CS8618

using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Numerics;
using System.Drawing;
using Shader = ProjectNewWorld.Core.Objects.OpenGL.Shader;
using Silk.NET.Input;
using ProjectNewWorld.Core.Cameras;
using ProjectNewWorld.Core.Input;
using ProjectNewWorld.Core.Objects;
using ProjectNewWorld.Core.Objects.OpenGL;
using Core;

namespace ProjectNewWorld.Core;

public class Game
{
    public readonly IWindow MainWindow;

    public GL GL { get; private set; }
    public GraphicsRenderer GraphicsRenderer { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public BaseCamera Camera { get; private set; }
    public ConsoleLogger ConsoleLogger { get; private set; }

    private IInputContext _input;

    private List<RenderableObject> _renderableObjects;

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
        MainWindow.Closing += OnClosing;
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

        GraphicsRenderer = new(this);
        GraphicsRenderer.SetViewport(new(1080, 750));

        vertShader = new(GL, "Assets\\Shaders\\plain_vert.shader", ShaderType.VertexShader);
        fragShader = new(GL, "Assets\\Shaders\\gradient_frag.shader", ShaderType.FragmentShader);

        shaderProgram = new(this, vertShader, fragShader);

        vertShader.Dispose();
        fragShader.Dispose();

        _renderableObjects = new()
        {
            new Triangle(this, new Transform(Vector3.Zero)),
            new Objects.Rectangle(this, new Transform(Vector3.UnitX)),
            new Voxel(this, new Transform(Vector3.UnitX * 2f)),
        };
    }

    private void OnClosing()
    {
        InputHandler.Dispose();
        foreach (RenderableObject obj in _renderableObjects)
        {
            obj.Dispose();
        }
    }

    private void OnUpdate(double delta)
    {
        if (InputHandler.Keyboard.IsKeyPressed(Key.Escape))
        {
            MainWindow.Close();
        }

        GraphicsRenderer.Update(delta);

        Camera.Update(delta);
    }

    private void OnRender(double delta)
    {
        GraphicsRenderer.Clear(Color.CornflowerBlue);

        List<Triangle> triangles = [.. _renderableObjects.OfType<Triangle>()];
        List<Objects.Rectangle> rectangles = [.. _renderableObjects.OfType<Objects.Rectangle>()];
        List<Voxel> voxels = [.. _renderableObjects.OfType<Voxel>()];

        foreach (var triangle in triangles) GraphicsRenderer.DrawTriangle(triangle, shaderProgram);
        foreach (var rectangle in rectangles) GraphicsRenderer.DrawRectangle(rectangle, shaderProgram);
        foreach (var voxel in voxels) GraphicsRenderer.DrawVoxel(voxel, shaderProgram);

        MainWindow.SwapBuffers();
    }
}
