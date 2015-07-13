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
        case MaterialType.BlinnPhongMaterial:
          return new BlinnPhongMaterial();
        case MaterialType.BlinnPhongColorTextureMaterial:
          throw new NotImplementedException();
        default:
          throw new ArgumentOutOfRangeException("type");
      }
    }
  }
}