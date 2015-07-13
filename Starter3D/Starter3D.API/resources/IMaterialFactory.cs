using Starter3D.API.utils;

namespace Starter3D.API.resources
{
  public interface IMaterialFactory
  {
    IMaterial CreateMaterial(MaterialType type);
  }
}