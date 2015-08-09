#version 330

precision highp float;

const int maxNumberOfLights = 10;
uniform float activeNumberOfPointLights;
uniform float activeNumberOfDirectionalLights;
uniform vec3 pointLightPositions[maxNumberOfLights];
uniform vec3 pointLightColors[maxNumberOfLights];
uniform vec3 directionalLightDirections[maxNumberOfLights];
uniform vec3 directionalLightColors[maxNumberOfLights];

uniform vec3 cameraPosition;

uniform vec3 ambientLight;
uniform vec3 diffuseColor;
uniform float n;
uniform float R;


in vec3 fragPosition;
in vec3 fragNormal;
in vec3 fragTextureCoords;

out vec4 outFragColor;

const float PI = 3.14159265358979323846;

float lump(vec3 h, float R, float n)
{
    return (n+1)/(PI*R*R) * pow(1-dot(h,h)/(R*R), n);
}


vec3 BRDF( vec3 N, vec3 L, vec3 V, vec3 H, vec3 diffuse)
{
    float NdotV = dot(N,V);
    float NdotL = dot(N,L);

    if (NdotL < 0 || NdotV < 0) return vec3(0);

    float NdotH = dot(N,H);
    float LdotH = dot(L,H);

    // scaling projection
    vec3 uH = L+V; // unnormalized H
    vec3 h = NdotV / dot(N,uH) * uH;
    vec3 huv = h - NdotV * N;

    // specular term (D and G)
    float p = lump(huv, R, n);
    return diffuse * vec3(p * pow(NdotV, 2) / (4 * NdotL * LdotH * pow(NdotH, 3)));
}

vec3 shade(vec3 p, vec3 n, vec3 diffuse)
{
  n = normalize(n);
  vec3 v = cameraPosition - p;
  v = normalize(v);

  vec3 color = ambientLight * diffuse;
  for(int pointLightIndex = 0; pointLightIndex < activeNumberOfPointLights; pointLightIndex++)
  {
	  vec3 l = pointLightPositions[pointLightIndex] - p;
	  l = normalize(l);
	  vec3 h = v + l;
	  h = normalize(h); 
	  color += pointLightColors[pointLightIndex] * BRDF(n, l, v, h, diffuse);
  }
  for(int directionalLightIndex = 0; directionalLightIndex < activeNumberOfDirectionalLights; directionalLightIndex++)
  {
	  vec3 l = -directionalLightDirections[directionalLightIndex];
	  l = normalize(l);
	  vec3 h = v + l;
	  h = normalize(h); 
	  color += directionalLightColors[directionalLightIndex] * BRDF(n, l, v, h, diffuse);
  }
  return color;
}  


void main(void)
{
  outFragColor = vec4(shade(fragPosition, fragNormal, diffuseColor), 1.0);
}