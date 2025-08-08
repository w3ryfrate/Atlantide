#version 460 core

out vec4 out_color;

uniform float uTime;      // Time in seconds

void main() {
    // Animate colors using sine waves
    float r = 0.5 + 0.5 * sin(uTime + 0.0);
    float g = 0.5 + 0.5 * sin(uTime + 2.0);
    float b = 0.5 + 0.5 * sin(uTime + 4.0);

    out_color = vec4(r, g, b, 1.0);
}
