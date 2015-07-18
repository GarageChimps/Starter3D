using System;
using Starter3D.API.utils;

namespace Starter3D.API.resources
{
  public class MaterialFactory : IMaterialFactory
  {
    public IMaterial CreateMaterial(MaterialType type)
    {
      switch (type)
      {
        case MaterialType.BaseMaterial:
          return  new Material();
        default:
          throw new ArgumentOutOfRangeException("type");
      }
    }
  }
}