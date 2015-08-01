#version 330
      
precision highp float;

in vec3 inPosition;
in vec3 inNormal;
in vec3 inTextureCoords;

out vec2 pixelCoords;
void main(void)
{
    gl_Position = vec4(inPosition, 1);
    pixelCoords = vec2((inPosition.x + 1.0)/2.0, (inPosition.y + 1.0)/2.0);
}
