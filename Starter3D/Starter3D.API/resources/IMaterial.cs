using ThreeAPI.renderer;

namespace ThreeAPI.resources
{
  public interface IMaterial
  {
    string VertexShader { get; }
    string FragmentShader { get; }
    void ConfigureRenderer(IRenderer renderer);
  }
}
