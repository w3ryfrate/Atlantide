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
using ProjectNewWorld.Core.Helpers;
using ProjectNewWorld.Core.Objects.OpenGL;

namespace ProjectNewWorld.Core;

public class GameEngine
{
    public readonly IWindow MainWindow;

    public GL GL { get; private set; }
    public GraphicsHandler GraphicsHandler { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public BaseCamera Camera { get; private set; }

    private IInputContext _input;

    private RenderableObject[] _renderableObjects = new RenderableObject[3];

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

        Camera = new FreeViewCamera(this, new(0, 0, 5), -Vector3.UnitZ, 10f);

        GraphicsHandler = new(this);
        GraphicsHandler.SetViewport(new(1080, 750));

        vertShader = new(GL, "Assets\\Shaders\\plain_vert.shader", ShaderType.VertexShader);
        fragShader = new(GL, "Assets\\Shaders\\gradient_frag.shader", ShaderType.FragmentShader);

        shaderProgram = new(GL);
        shaderProgram.AttachShader(vertShader);
        shaderProgram.AttachShader(fragShader);
        shaderProgram.Link();
        
        shaderProgram.DetachShader(vertShader);
        shaderProgram.DetachShader(fragShader);
        vertShader.Dispose();
        fragShader.Dispose();
        
        for (int i = 0; i < _renderableObjects.Length; i++)
        {
            if (i == 0)
            {
                Objects.Triangle triangle = new(this, new(Vector3.Zero));
                _renderableObjects[i] = triangle;
            }
            else
            {
                Objects.Rectangle rect = new( this, new(Vector3.UnitX * i));
                _renderableObjects[i] = rect;
            }
        }

        _renderableObjects[2].Transform.Rotation = new(0f, MathHelper.ToRadians(180f), 0f);
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
        GraphicsHandler.Clear(Color.CornflowerBlue);

        foreach (var obj in _renderableObjects)
        {
            if (obj is Objects.Rectangle)
                GraphicsHandler.DrawRectangle(obj as Objects.Rectangle, shaderProgram, Color.Firebrick);
            else
                GraphicsHandler.DrawTriangle(obj as Triangle, shaderProgram, Color.Sienna);
        }

        MainWindow.SwapBuffers();
    }
}
