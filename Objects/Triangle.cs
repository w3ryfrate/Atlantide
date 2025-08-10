using ProjectNewWorld.Core.GLObjects;
using Silk.NET.OpenGL;
using System.Numerics;

namespace ProjectNewWorld.Core.Objects;

public class Triangle : RenderableObject
{
    protected override float[] Vertices => _vertices;
    private readonly float[] _vertices =
    {
       -0.5f,-0.5f, 0.0f,
        0.5f,-0.5f, 0.0f,
        0.0f, 0.5f, 0.0f,
    };

    public Triangle(ShaderProgram program, GameEngine engine, Vector3 position) : base(program, engine, position)
    {
        VAO.Bind();

        VBO.Bind();
        unsafe
        {
            fixed (float* ptr = _vertices)
                VBO.SetData((nuint)(_vertices.Length * sizeof(float)), ptr, BufferUsageARB.StaticDraw);
        }

        gl.EnableVertexAttribArray(0);
        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        VAO.Unbind();
        VBO.Unbind();
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);
    }

    public override void Draw()
    {
        base.Draw();
        gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}
