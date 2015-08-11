using System.Collections.Generic;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public interface IResourceManager
  {
    void Load(string resourceFile);
    void Configure(IRenderer renderer);
    
    IMaterial GetMaterial(string key);
    IEnumerable<IMaterial> GetMaterials(); 
    void AddMaterial(string key, IMaterial material);
    bool HasMaterial(string key);

    IShader GetShader(string key);
    void AddShader(string key, IShader shader);
    IEnumerable<IShader> GetShaders();
    bool HasShader(string key);

    ITexture GetTexture(string key);
    void AddTexture(string key, ITexture texture);
    IEnumerable<ITexture> GetTextures();
    bool HasTexture(string key);

  }
}