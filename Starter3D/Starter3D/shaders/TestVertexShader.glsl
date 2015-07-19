#version 330

in vec3 inPosition;
in vec3 inNormal;
out vec3 fragPosition;
out vec3 fragNormal;

precision highp float;

void main(void) {
   fragPosition = inPosition;
   fragNormal = inNormal;
   gl_Position = vec4(fragPosition, 1);
}

