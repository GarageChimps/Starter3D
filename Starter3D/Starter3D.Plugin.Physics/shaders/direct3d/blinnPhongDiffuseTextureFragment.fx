SamplerState BilinearTextureSampler
{
  Filter = MIN_MAG_MIP_LINEAR;
  AddressU = Wrap;
  AddressV = Wrap;
};

struct vertexAttributes {
  float3 inPosition : POSITION;
  float3 inNormal : NORMAL;
  float3 inTextureCoords: TEXCOORD0;
};

struct fragmentAttributes {
  float4 position : SV_POSITION;
  float3 fragPosition : TEXCOORD0;
  float3 fragNormal : NORMAL;
  float3 fragTextureCoords : TEXCOORD1;
};

static const int maxNumberOfLights = 10;

uniform float4x4 projectionMatrix;
uniform float4x4 viewMatrix;
uniform float4x4 modelMatrix;

uniform float3 pointLightPositions[maxNumberOfLights];
uniform float3 pointLightColors[maxNumberOfLights];

uniform float3 directionalLightDirections[maxNumberOfLights];
uniform float3 directionalLightColors[maxNumberOfLights];

uniform float activeNumberOfPointLights;
uniform float activeNumberOfDirectionalLights;

uniform float3 cameraPosition;

uniform float3 ambientLight;
uniform float3 specularColor;
uniform float shininess;

uniform Texture2D diffuseTexture;

float3 lambertBRDF(float3 normal, float3 lightDirection, float3 color)
{
  return color * max(dot(normal, lightDirection), 0.0);
}

float3 blinnPhongBRDF(float3 normal, float3 halfVector, float3 color, float shininess)
{
  return color * pow(max(dot(normal, halfVector), 0.0), shininess);
}

float3 BRDF(float3 normal, float3 lightDirection, float3 viewDirection, float3 halfVector, float3 diffuse)
{
  return lambertBRDF(normal, lightDirection, diffuse) + blinnPhongBRDF(normal, halfVector, specularColor, shininess);
}

float3 shade(float3 p, float3 n, float3 diffuse)
{
  n = normalize(n);
  float3 v = cameraPosition - p;
  v = normalize(v);

  float3 color = ambientLight * diffuse;
  for (int pointLightIndex = 0; pointLightIndex < activeNumberOfPointLights; pointLightIndex++)
  {
    float3 l = pointLightPositions[pointLightIndex] - p;
    l = normalize(l);
    float3 h = v + l;
    h = normalize(h);
    color += pointLightColors[pointLightIndex] * BRDF(n, l, v, h, diffuse);
  }
  for (int directionalLightIndex = 0; directionalLightIndex < activeNumberOfDirectionalLights; directionalLightIndex++)
  {
    float3 l = -directionalLightDirections[directionalLightIndex];
    l = normalize(l);
    float3 h = v + l;
    h = normalize(h);
    color += directionalLightColors[directionalLightIndex] * BRDF(n, l, v, h, diffuse);
  }
  return color;
}


fragmentAttributes VShader(vertexAttributes input)
{
  fragmentAttributes output = (fragmentAttributes)0;
  float4 worldPosition = mul(float4(input.inPosition, 1), modelMatrix);
  float4 worldNormal = mul(float4(input.inNormal, 0), modelMatrix);
  output.fragPosition = worldPosition.xyz;
  output.fragNormal = worldNormal.xyz;
  output.fragTextureCoords = input.inTextureCoords;
  output.position = mul(mul(worldPosition, viewMatrix), projectionMatrix);
  return output;
}


float4 FShader(fragmentAttributes input) : SV_Target
{  
  float3 diffuseColor = diffuseTexture.Sample(BilinearTextureSampler, input.fragTextureCoords.xy);
  return float4(shade(input.fragPosition, input.fragNormal, diffuseColor), 1.0);
}


technique10 Render
{

  pass P0
  {
    SetGeometryShader(0);
    SetVertexShader(CompileShader(vs_4_0, VShader()));
    SetPixelShader(CompileShader(ps_4_0, FShader()));
  }
}
