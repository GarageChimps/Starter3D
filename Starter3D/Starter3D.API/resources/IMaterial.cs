using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public interface IMaterial
  {
    string VertexShader { get; }
    string FragmentShader { get; }
    void ConfigureRenderer(IRenderer renderer);
  }
}
