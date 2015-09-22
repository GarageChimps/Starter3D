#version 330

precision highp float;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

in vec3 inPosition;
in vec3 inNormal;
in vec3 inTextureCoords;
in mat4 instanceMatrix;

out vec3 fragPosition;
out vec3 fragNormal;
out vec3 fragTextureCoords;

void main(void)
{
  mat4 modelInstanceMatrix = instanceMatrix * modelMatrix;
  vec4 worldPosition = modelInstanceMatrix * vec4(inPosition,1);
  vec4 worldNormal = modelInstanceMatrix * vec4(inNormal,0);
  fragPosition = worldPosition.xyz;
  fragNormal = normalize(worldNormal.xyz);
  fragTextureCoords = inTextureCoords;
  gl_Position = projectionMatrix * viewMatrix * worldPosition;
}