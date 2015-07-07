#version 330

in vec3 inPosition;
out vec3 fragPosition;

precision highp float;

void main(void) {
   fragPosition = inPosition;
   gl_Position = vec4(fragPosition, 1);
}

