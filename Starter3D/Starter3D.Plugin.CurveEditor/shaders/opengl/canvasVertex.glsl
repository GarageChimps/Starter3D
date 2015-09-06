#version 330
      
precision highp float;

in vec3 inPosition;

void main(void)
{
	
    gl_Position = vec4(inPosition, 1);
}
