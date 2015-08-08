struct vertexAttributes {
  float3 inPosition : POSITION;
  float3 inNormal : NORMAL;
  float3 inTextureCoords: TEXCOORD0;
};

struct fragmentAttributes {
  float4 position : SV_POSITION;
  float2 pixelCoords : TEXCOORD0;
};


fragmentAttributes VShader(vertexAttributes input)
{
    fragmentAttributes output = (fragmentAttributes)0;
    output.position = float4(input.inPosition, 1);
    output.pixelCoords = float2((input.inPosition.x + 1.0) / 2.0, (input.inPosition.y + 1.0) / 2.0);
    return output;
}
