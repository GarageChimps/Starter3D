#version 330

precision highp float;

struct PointLight
{
	vec3 Position;
	vec3 Color;
};

struct DirectionalLight
{
	vec3 Direction;
	vec3 Color;
};

const int maxNumberOfLights = 10;
uniform float activeNumberOfPointLights;
uniform float activeNumberOfDirectionalLights;
uniform PointLight pointLights[maxNumberOfLights];
uniform DirectionalLight directionalLights[maxNumberOfLights];

uniform vec3 cameraPosition;

uniform vec3 ambientLight;
uniform vec3 diffuseColor;

//Add aditional parameters here

in vec3 fragPosition;
in vec3 fragNormal;
in vec3 fragTextureCoords;

out vec4 outFragColor;


vec3 BRDF( vec3 N, vec3 V, vec3 L, vec3 diffuse)
{
    //Implement BRDF here
    return vec3(1,1,1);
}


vec3 shade(vec3 p, vec3 n, vec3 diffuse)
{
  n = normalize(n);
  vec3 v = cameraPosition - p;
  v = normalize(v);

  vec3 color = ambientLight * diffuse;
  for(int pointLightIndex = 0; pointLightIndex < activeNumberOfPointLights; pointLightIndex++)
  {
	  vec3 l = pointLights[pointLightIndex].Position - p;
	  l = normalize(l);
	  color += pointLights[pointLightIndex].Color * BRDF(n, v, l, diffuse);
  }
  for(int directionalLightIndex = 0; directionalLightIndex < activeNumberOfDirectionalLights; directionalLightIndex++)
  {
	  vec3 l = -directionalLights[directionalLightIndex].Direction;
	  l = normalize(l);
	  color += directionalLights[directionalLightIndex].Color * BRDF(n, v, l, diffuse);
  }
  return color;
}  


void main(void)
{
  outFragColor = vec4(shade(fragPosition, fragNormal, diffuseColor), 1.0);
}