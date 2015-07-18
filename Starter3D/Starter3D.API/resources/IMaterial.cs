using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public interface IMaterial
  {
    string ShaderName { get; }
    void Configure(IRenderer renderer);
    void UseMaterial(IRenderer renderer);
    void Load(IDataNode dataNode);
  }
}
