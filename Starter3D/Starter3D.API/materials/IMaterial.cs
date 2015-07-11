using ThreeAPI.renderer;

namespace ThreeAPI.materials
{
  public interface IMaterial
  {
    string VertexShader { get; }
    string FragmentShader { get; }
  }
}
