#version 460 core

in vec2 fragUV;
out vec4 outColor;

uniform vec4 uColor1;
uniform vec4 uColor2;

//uniform vec2 uViewportSize;

void main() {
    vec4 finalColor = mix(uColor1, uColor2, fragUV.y);
    outColor = finalColor;
}
