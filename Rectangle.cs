using ProjectNewWorld.Core.Objects;
using Silk.NET.OpenGL;
using System.Numerics;

namespace ProjectNewWorld.Core;

public class Rectangle : RenderableObject
{
    private BufferObject _ebo;
    private const float _zApos = -1f;

    private Matrix4x4 _view;
    private Matrix4x4 _projection;

    protected override float[] Vertices => _vertices;
    private float[] _vertices =
    {
        //aPosition     
         0.5f,  0.5f, _zApos,
         0.5f, -0.5f, _zApos,
        -0.5f, -0.5f, _zApos,
        -0.5f,  0.5f, _zApos,   
    };

    private readonly uint[] indices =
    {
        0u, 1u, 3u,
        1u, 2u, 3u
    };

    public Rectangle(GL gl, ShaderProgram program, Matrix4x4 model, GraphicsHandler graphics) : base(gl, graphics, program, model)
    {
        Model = Matrix4x4.Identity;

        VAO.Bind();

        VBO.Bind();
        unsafe
        {
            fixed (float* ptr = Vertices)
                VBO.SetData((nuint)(Vertices.Length * sizeof(float)), ptr, BufferUsageARB.StaticDraw);
        }

        _ebo = new(gl, BufferTargetARB.ElementArrayBuffer);
        _ebo.Bind();
        unsafe
        {
            fixed (uint* ptr = indices)
                _ebo.SetData((nuint)(indices.Length * sizeof(uint)), ptr, BufferUsageARB.StaticDraw);
        }

        gl.EnableVertexAttribArray(0);
        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        _view = Matrix4x4.CreateLookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);
        _projection = Matrix4x4.CreatePerspectiveFieldOfView(1.3f, Graphics.Viewport.GetAspectRatio(), 0.1f, 100f);

        VAO.Unbind();
        VBO.Unbind();
        _ebo.Unbind();
    }

    float time = 0f;
    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);
        time += (float)deltaTime;
        ShaderProgram.SetUniform1("uTime", time);
    }

    public override void Draw()
    {
        base.Draw();
        ShaderProgram.SetUniformMat4("uView", _view);
        ShaderProgram.SetUniformMat4("uProjection", _projection);
        ShaderProgram.Use();
        VAO.Bind();
        unsafe
        {
            gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        if (disposing)
        {
            Array.Clear(indices);
        }

        _ebo.Dispose();
        base.Dispose(disposing);
    }
}
