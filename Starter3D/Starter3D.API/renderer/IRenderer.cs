using ThreeAPI.geometry;

namespace ThreeAPI.renderer
{
  public interface IRenderer
  {
    void ConfigureMesh(IMesh mesh);
    void Render(IMesh mesh);
  }
}
