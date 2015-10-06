using System;
using Starter3D.API.utils;

namespace Starter3D.API.geometry.primitives
{
  public class PrimitiveFactory : IPrimitiveFactory
  {
    public IMesh CreatePrimitive(PrimitiveType primitiveType, string primitiveName)
    {
      switch (primitiveType)
      {
        case PrimitiveType.Cube:
          return new Cube(primitiveName);
        case PrimitiveType.Sphere:
          return new Sphere(10, 10);
        default:
          throw new ArgumentOutOfRangeException("primitiveType");
      }
    }
  }
}