using System.Drawing;

namespace ProjectNewWorld.Core;

public readonly struct Viewport
{
    public readonly Size Size;

    public Viewport(int width, int height)
    {
        Size = new(width, height);
    }
    public Viewport(Size size)
    {
        Size = size;
    }

    public float GetAspectRatio() => (float)Size.Width / (float)Size.Height;
}
