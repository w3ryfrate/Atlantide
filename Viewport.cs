namespace ProjectNewWorld.Core;

public readonly struct Viewport
{
    public readonly int Width;
    public readonly int Height;

    public Viewport(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public float GetAspectRatio() => (float)Width / (float)Height;
}
