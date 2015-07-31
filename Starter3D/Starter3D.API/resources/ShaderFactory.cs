using System;
using Starter3D.API.utils;

namespace Starter3D.API.resources
{
  public class ShaderFactory : IShaderFactory
  {
    public IShader CreateShader(ShaderResourceType type)
    {
      switch (type)
      {
        case ShaderResourceType.BaseShader:
          return new Shader();
        default:
          throw new ArgumentOutOfRangeException("type");
      }
    }
  }
}