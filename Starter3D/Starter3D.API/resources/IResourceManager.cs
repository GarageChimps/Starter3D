using Starter3D.API.geometry;

namespace Starter3D.API.resources
{
  public interface IResourceManager
  {
    void Load(string resourceFile);
    IMaterial GetMaterial(string key);
    void AddMaterial(string key, IMaterial material);
  }
}