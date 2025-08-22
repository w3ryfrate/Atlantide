#pragma warning disable CS8618

using Silk.NET.Input;

namespace Core.Input;

public class InputHandler
{
    private readonly IInputContext _input;
    public readonly IMouse Mouse;
    public readonly IKeyboard Keyboard;

    public InputHandler(IInputContext inputContext)
    {
        _input = inputContext;
        Keyboard = _input.Keyboards[0];
        Mouse = _input.Mice[0];
    }
}
