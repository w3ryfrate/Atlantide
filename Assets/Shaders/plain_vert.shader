#version 460 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTextureUV;

out vec2 fragUV;
uniform mat4 uMVP;

void main()
{
    gl_Position = vec4(aPosition, 1.0) * uMVP;
    fragUV = aTextureUV;
}