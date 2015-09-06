struct vertexAttributes {
  float3 inPosition : POSITION;
  float3 inNormal : NORMAL;
  float3 inTextureCoords: TEXCOORD0;
};

struct fragmentAttributes {
  float4 position : SV_POSITION;
  float2 pixelCoords : TEXCOORD0;
};


static const int number_of_iterations = 100;
static const float3 colorIn = float3(1, 0.75, 0.5);
static const float3 colorOut = float3(0, 0.5, 1);

uniform float3 mouse;
uniform float time;

float3 palette(float t, float3 a, float3 b, float3 c, float3 d)
{
  return a + b*cos(6.28318*(c*t + d));
}

float2 iteration(float2 z, float2 c)
{
  return float2(z.x*z.x - z.y*z.y + c.x, 2 * z.x*z.y + c.y);
}


fragmentAttributes VShader(vertexAttributes input)
{
  fragmentAttributes output = (fragmentAttributes)0;
  output.position = float4(input.inPosition, 1);
  output.pixelCoords = float2((input.inPosition.x + 1.0) / 2.0, (input.inPosition.y + 1.0) / 2.0);
  return output;
}


float4 FShader(fragmentAttributes input) : SV_Target
{
  float2 c = 4 * (input.pixelCoords) - 2;
  float zoom = 1;

  float2 z = float2(0, 0);

    float finalIt = number_of_iterations;
  for (int it = 0; it < number_of_iterations; it++)
  {
    z = iteration(z, c);
    if (length(z) > 2)
      finalIt = it;
  }

  float interpolator = (number_of_iterations - finalIt) / number_of_iterations;
  float3 color = palette(interpolator, float3(0.5, 0.5, 0.5), float3(0.5, 0.5, 0.5), float3(1.0, 1.0, 1.0), float3(0.0, 0.1, 0.2));
    return float4(color, 1);
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
