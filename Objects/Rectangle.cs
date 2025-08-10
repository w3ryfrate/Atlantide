using ProjectNewWorld.Core.GLObjects;
using Silk.NET.OpenGL;
using System.Numerics;

namespace ProjectNewWorld.Core.Objects;

public class Rectangle : RenderableObject
{
    private BufferObject _ebo;

    protected override float[] Vertices => _vertices;
    private float[] _vertices =
    {
        //aPosition     
         0.5f,  0.5f, 0.0f,
         0.5f, -0.5f, 0.0f,
        -0.5f, -0.5f, 0.0f,
        -0.5f,  0.5f, 0.0f,   
    };

    private readonly uint[] indices =
    {
        0u, 1u, 3u,
        1u, 2u, 3u
    };

    public Rectangle(ShaderProgram program, GameEngine engine, Vector3 position) : base(program, engine, position)
    {
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

        VAO.Unbind();
        VBO.Unbind();
        _ebo.Unbind();
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);
    }

    public override void Draw()
    {
        base.Draw();
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
