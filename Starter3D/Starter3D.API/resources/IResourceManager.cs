using System.Collections.Generic;

namespace Starter3D.API.resources
{
  public interface IResourceManager
  {
    void Load(string resourceFile);
    IMaterial GetMaterial(string key);
    IEnumerable<IMaterial> GetMaterials(); 
    void AddMaterial(string key, IMaterial material);
    IShader GetShader(string key);
    void AddShader(string key, IShader shader);
  }
}