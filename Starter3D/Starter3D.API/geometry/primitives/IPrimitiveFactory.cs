using Starter3D.API.utils;

namespace Starter3D.API.geometry.primitives
{
  public interface IPrimitiveFactory
  {
    IMesh CreatePrimitive(PrimitiveType primitiveType, string name);
  }
}