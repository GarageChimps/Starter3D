using Starter3D.API.utils;

namespace Starter3D.API.resources
{
  public interface ITextureFactory
  {
    ITexture CreateTexture(TextureType type);
  }
}