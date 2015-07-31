using Starter3D.API.utils;

namespace Starter3D.API.resources
{
  public interface IShaderFactory
  {
    IShader CreateShader(ShaderResourceType type);
  }
}