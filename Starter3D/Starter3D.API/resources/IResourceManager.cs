using Starter3D.API.geometry;

namespace Starter3D.API.resources
{
  public interface IResourceManager
  {
    void Load(string resourceFile);

    IMaterial GetMaterial(string key);
    IShape GetShape(string key);
    string GetShader(string key);

    void AddMaterial(string key, IMaterial material);
    void AddShape(string key, IShape shape);
    void AddShader(string key, string shader);
  }
}