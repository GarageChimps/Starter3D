﻿#version 330

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
uniform vec3 specularColor;
uniform float shininess;


in vec3 fragPosition;
in vec3 fragNormal;
in vec3 fragTextureCoords;

out vec4 outFragColor;

vec3 lambertBRDF(vec3 normal, vec3 lightDirection, vec3 color)
{
	return color * max(dot(normal,lightDirection),0.0);
}

vec3 blinnPhongBRDF(vec3 normal, vec3 halfVector, vec3 color, float shininess)
{
	return color * pow(max(dot(normal,halfVector),0.0), shininess);
}

vec3 BRDF(vec3 normal, vec3 lightDirection, vec3 halfVector, vec3 diffuse, vec3 specular, float shininess)
{
	return lambertBRDF(normal, lightDirection, diffuse) + blinnPhongBRDF(normal, halfVector, specular, shininess);
}

vec3 shade(vec3 p, vec3 n, vec3 diffuse, vec3 specular, float shininess)
{
  n = normalize(n);
  vec3 v = cameraPosition - p;
  v = normalize(v);

  vec3 color = ambientLight * diffuse;
  for(int pointLightIndex = 0; pointLightIndex < activeNumberOfPointLights; pointLightIndex++)
  {
	  vec3 l = pointLights[pointLightIndex].Position - p;
	  l = normalize(l);
	  vec3 h = v + l;
	  h = normalize(h); 
	  color += pointLights[pointLightIndex].Color * BRDF(n, l, h, diffuse, specular, shininess);
  }
  for(int directionalLightIndex = 0; directionalLightIndex < activeNumberOfDirectionalLights; directionalLightIndex++)
  {
	  vec3 l = -directionalLights[directionalLightIndex].Direction;
	  l = normalize(l);
	  vec3 h = v + l;
	  h = normalize(h); 
	  color += directionalLights[directionalLightIndex].Color * BRDF(n, l, h, diffuse, specular, shininess);
  }
  return color;
}  


void main(void)
{
  outFragColor = vec4(shade(fragPosition, fragNormal, diffuseColor, specularColor, shininess), 1.0);
}