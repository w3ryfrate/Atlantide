using ProjectNewWorld.Core.Helpers;
using ProjectNewWorld.Core.Objects;
using ProjectNewWorld.Core.Objects.OpenGL;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Numerics;

namespace ProjectNewWorld.Core;

public enum ShaderProgramType
{
    PLAIN_PLAIN,
    PLAIN_GRADIENT,
}

public class GraphicsRenderer
{
    public Matrix4x4 Projection { get; private set; }
    public Viewport Viewport { get; private set; }

    private List<RenderableObject> _toUpdate = new();

    private readonly Game _game;
    private readonly GL _gl;
    private readonly Dictionary<ShaderProgramType, ShaderProgram> _shaderPrograms;
    private const string SHADERS_DIRECTORY = "Assets\\Shaders\\";

    public GraphicsRenderer(Game game)
    {
        _game = game;
        _gl = _game.GL;
        _gl.Enable(EnableCap.DepthTest);

        //Objects.OpenGL.Shader plainVertShader = new(_gl, SHADERS_DIRECTORY + "plain_vert.shader", ShaderType.VertexShader);

        //Objects.OpenGL.Shader plainFragShader = new(_gl, SHADERS_DIRECTORY + "plain_frag.shader", ShaderType.FragmentShader);
        //Objects.OpenGL.Shader gradientShader = new(_gl, SHADERS_DIRECTORY + "gradient_frag.shader", ShaderType.FragmentShader);

        //_shaderPrograms = new()
        //{
        //    { ShaderProgramType.PLAIN_PLAIN, new(game, plainVertShader, plainFragShader) },
        //    { ShaderProgramType.PLAIN_GRADIENT, new(game, plainVertShader, gradientShader) }
        //};
    }

    public void SetViewport(Viewport viewport)
    {
        Viewport = viewport;
        _game.MainWindow.Size = new(this.Viewport.Size.Width, this.Viewport.Size.Height);
        Projection = Matrix4x4.CreatePerspectiveFieldOfView(_game.Camera.FieldOfView, Viewport.GetAspectRatio(), 0.1f, 100f);
        _gl.Viewport(0, 0, (uint)Viewport.Size.Width, (uint)Viewport.Size.Height);
    }

    public void Clear(Color color)
    {
        _gl.ClearColor(color);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void Update(double delta)
    {
        foreach (var obj in _toUpdate)
        {
            if (!obj.Disposed)
                obj.Update(delta);
        }

        _toUpdate.Clear();
    }

    public void DrawVoxel(Voxel voxel, ShaderProgram shaderProgram)
    {
        if (voxel.Disposed)
            return;

        _toUpdate.Add(voxel);
        UseProgram(voxel, shaderProgram);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }

    public void DrawRectangle(Objects.Rectangle rect, ShaderProgram shaderProgram)
    {
        if (rect.Disposed)
            return;

        _toUpdate.Add(rect);
        UseProgram(rect, shaderProgram);
        unsafe
        {
            _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0);
        }
    }

    public void DrawTriangle(Triangle triangle, ShaderProgram shaderProgram)
    {
        if (triangle.Disposed)
            return;

        _toUpdate.Add(triangle);
        UseProgram(triangle, shaderProgram);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    private void UseProgram(RenderableObject obj, ShaderProgram shaderProgram)
    {
        shaderProgram.SetUniformMat4("uMVP", obj.Model * _game.Camera.View * Projection);
        shaderProgram.SetUniform4("uColor1", Color.Red.ToVector4());
        shaderProgram.SetUniform4("uColor2", Color.Blue.ToVector4());
        shaderProgram.Use();
        obj.VAO.Bind();
    }
}
