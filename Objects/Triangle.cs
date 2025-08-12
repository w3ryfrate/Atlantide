using Silk.NET.OpenGL;

namespace ProjectNewWorld.Core.Objects;

public class Triangle : RenderableObject
{
    protected override float[] VertexBufferData => _vertices;
    private readonly float[] _vertices =
    {
       -0.5f,-0.5f, 0.0f,   0.0f, 0.0f,
        0.5f,-0.5f, 0.0f,   1.0f, 0.0f,
        0.0f, 0.5f, 0.0f,   0.5f, 1.0f,
    };

    public Triangle(GameEngine engine, Transform transform) : base(engine, transform)
    {
        VAO.Bind();

        VBO.Bind();
        unsafe
        {
            fixed (float* ptr = _vertices)
                VBO.BufferData((nuint)(_vertices.Length * sizeof(float)), ptr, BufferUsageARB.StaticDraw);
        }

        gl.EnableVertexAttribArray(0);
        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        gl.EnableVertexAttribArray(1);
        gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        VAO.Unbind();
        VBO.Unbind();
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);
    }

    protected override void OnBeforeDraw(object? sender, EventArgs e)
    {
        base.OnBeforeDraw(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}
