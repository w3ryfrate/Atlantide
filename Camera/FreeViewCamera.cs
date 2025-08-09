using Silk.NET.Input;
using System.Numerics;

namespace ProjectNewWorld.Core.Camera;

public class FreeViewCamera : BaseCamera
{
    public FreeViewCamera(Vector3 initialPosition) : base(initialPosition)
    {
        Target = -Vector3.UnitZ;
    }

    public override void Update(IKeyboard keyboard)
    {
        base.Update(keyboard);
    }
}
