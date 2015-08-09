RasterizerState RSFillBackCull
{
  FillMode = SOLID;
  CullMode = None;
  DepthBias = false;
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

float4x4 projectionMatrix;
float4x4 viewMatrix;
float4x4 modelMatrix;

float3 pointLightPositions[maxNumberOfLights];
float3 pointLightColors[maxNumberOfLights];

float3 directionalLightDirections[maxNumberOfLights];  
float3 directionalLightColors[maxNumberOfLights];

float activeNumberOfPointLights;
float activeNumberOfDirectionalLights;

float3 cameraPosition;

float3 ambientLight;
float3 diffuseColor;
float3 specularColor;
float shininess;

float3 lambertBRDF(float3 normal, float3 lightDirection, float3 color)
{
  return color * max(dot(normal, lightDirection), 0.0);
}

float3 blinnPhongBRDF(float3 normal, float3 halfVector, float3 color, float shininess)
{
  return color * pow(max(dot(normal, halfVector), 0.0), shininess);
}

float3 BRDF(float3 normal, float3 lightDirection, float3 halfVector, float3 diffuse, float3 specular, float shininess)
{
  return lambertBRDF(normal, lightDirection, diffuse) + blinnPhongBRDF(normal, halfVector, specular, shininess);
}

float3 shade(float3 p, float3 n, float3 diffuse, float3 specular, float shininess)
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
    color += pointLightColors[pointLightIndex] * BRDF(n, l, h, diffuse, specular, shininess);
  }
  for (int directionalLightIndex = 0; directionalLightIndex < activeNumberOfDirectionalLights; directionalLightIndex++)
  {
    float3 l = -directionalLightDirections[directionalLightIndex];
    l = normalize(l);
    float3 h = v + l;
    h = normalize(h);
    color += directionalLightColors[directionalLightIndex] * BRDF(n, l, h, diffuse, specular, shininess);
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
  return float4(shade(input.fragPosition, input.fragNormal, diffuseColor, specularColor, shininess), 1.0);
}


technique10 Render
{

  pass P0
  {
    SetRasterizerState(RSFillBackCull);
    SetGeometryShader(0);
    SetVertexShader(CompileShader(vs_4_0, VShader()));
    SetPixelShader(CompileShader(ps_4_0, FShader()));
  }
}
