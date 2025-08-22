using Core.Objects.OpenGL;
using Silk.NET.OpenGL;

namespace Core.Objects;

public class Rectangle : RenderableObject
{
    private BufferObject _ebo;

    protected override float[] VertexBufferData => _vertices;
    private readonly float[] _vertices =
    {
        //aPosition            //aTextureUV
        -0.5f,-0.5f, 0.0f,    0.0f, 0.0f,
         0.5f,-0.5f, 0.0f,    1.0f, 0.0f,
        -0.5f, 0.5f, 0.0f,    0.0f, 1.0f,
         0.5f, 0.5f, 0.0f,    1.0f, 1.0f,
    };

    private readonly uint[] indices =
    {
        0u, 1u, 3u,
        0u, 3u, 2u
    };

    public Rectangle(Game game, Transform transform) : base(game, transform)
    {
        VAO.Bind();

        VBO.Bind();
        unsafe
        {
            fixed (float* ptr = VertexBufferData)
                VBO.BufferData((nuint)(VertexBufferData.Length * sizeof(float)), ptr, BufferUsageARB.StaticDraw);
        }

        _ebo = new(gl, BufferTargetARB.ElementArrayBuffer);
        _ebo.Bind();
        unsafe
        {
            fixed (uint* ptr = indices)
                _ebo.BufferData((nuint)(indices.Length * sizeof(uint)), ptr, BufferUsageARB.StaticDraw);
        }

        gl.EnableVertexAttribArray(0);
        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        gl.EnableVertexAttribArray(1);
        gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        VAO.Unbind();
        VBO.Unbind();
        _ebo.Unbind();
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
