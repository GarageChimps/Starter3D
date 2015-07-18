#version 330

precision highp float;

uniform vec3 lightPosition;
uniform vec3 lightColor;
uniform vec3 eye;

uniform vec3 ambientLight;
uniform vec3 diffuseColor;
uniform vec3 specularColor;
uniform float shininess;


in vec3 fragPosition;
in vec3 fragNormal;
in vec3 fragTextureCoords;

out vec4 outFragColor;

vec3 blinnPhongShading(vec3 p, vec3 n, vec3 diffuse, vec3 specular, float shininess, vec3 lightPosition, vec3 lightColor)
{
  n = normalize(n);
  vec3 v = eye - p;
  v = normalize(v);
  vec3 l = lightPosition - p;
  l = normalize(l);
  vec3 h = v + l;
  h = normalize(h); 
  vec3 shadedColor = lightColor * (diffuse * max(dot(n,l),0.0) + specular * pow(max(dot(n,h),0.0), shininess));  
  return shadedColor;
}  

vec3 shade(vec3 p, vec3 n, vec3 diffuse, vec3 specular, float shininess)
{
  vec3 color = blinnPhongShading(p,n,diffuse, specular, shininess, lightPosition, lightColor); 
  return ambientLight * diffuse + color;
}  


void main(void)
{
  outFragColor = vec4(shade(fragPosition, fragNormal, diffuseColor, specularColor, shininess), 1.0);
}