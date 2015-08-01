using System;
using Starter3D.API.utils;

namespace Starter3D.API.resources
{
  public class TextureFactory : ITextureFactory
  {
    public ITexture CreateTexture(TextureType type)
    {
      switch (type)
      {
        case TextureType.BaseTexture:
          return new Texture();
        default:
          throw new ArgumentOutOfRangeException("type");
      }
    }
  }
}