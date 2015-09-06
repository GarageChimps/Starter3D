#version 330

precision highp float;

out vec4 outFragColor;

uniform vec3 color;
      
void main(void)
{
  outFragColor = vec4(color, 1);
}