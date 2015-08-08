struct fragmentAttributes {
  float4 position : SV_POSITION;
  float2 pixelCoords : TEXCOORD0;
};

float4 FShader(fragmentAttributes input) : SV_Target
{
  return float4(1, 0, 0, 1);
}
