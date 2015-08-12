struct vertexAttributes {
  float3 inPosition : POSITION;
  float3 inNormal : NORMAL;
  float3 inTextureCoords: TEXCOORD0;
};

struct fragmentAttributes {
  float4 position : SV_POSITION;
};


fragmentAttributes VShader(vertexAttributes input)
{
  fragmentAttributes output = (fragmentAttributes)0;
  output.position = float4(input.inPosition, 1);
  return output;
}


float4 FShader(fragmentAttributes input) : SV_Target
{
  return float4(0, 1, 0, 1);
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
