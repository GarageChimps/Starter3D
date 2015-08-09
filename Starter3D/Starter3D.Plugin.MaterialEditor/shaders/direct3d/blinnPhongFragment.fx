RasterizerState RSFillBackCull
{
  FillMode = SOLID;
  CullMode = Back;
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

float4x4 projectionMatrix;
float4x4 viewMatrix;
float4x4 modelMatrix;

struct PointLight
{
  float3 Position;
  float3 Color;
};

struct DirectionalLight
{
  float3 Direction;
  float3 Color;
};

static const int maxNumberOfLights = 10;
float activeNumberOfPointLights;
float activeNumberOfDirectionalLights;
PointLight pointLights[maxNumberOfLights];
DirectionalLight directionalLights[maxNumberOfLights];

float3 cameraPosition;

float3 ambientLight;
float3 diffuseColor;
float3 specularColor;
float shininess;

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
  return float4(1,0,0, 1);
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
