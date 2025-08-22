
using Silk.NET.OpenGL;

namespace Core.Objects.OpenGL;

public static class ShaderProgramFactory
{
    private static Dictionary<ShaderProgramType, ShaderProgram> _shaderPrograms;
    private const string SHADERS_PATH = "Assets\\Shaders\\";

    private static GL gl;

    public static void CreatePrograms(Game game)
    {
        gl = game.GL;
        Shader plainVertShader = new(game, SHADERS_PATH + "plain_vert.shader", ShaderType.VertexShader);

        Shader plainFragShader = new(game, SHADERS_PATH + "plain_frag.shader", ShaderType.FragmentShader);
        Shader gradientFragShader = new(game, SHADERS_PATH + "gradient_frag.shader", ShaderType.FragmentShader);

        _shaderPrograms = new()
        {
            { ShaderProgramType.PLAIN_PLAIN, new(game, plainVertShader,plainFragShader)},
            { ShaderProgramType.PLAIN_GRADIENT, new(game, plainVertShader,gradientFragShader)},
        };
    }
}
