using ThreeAPI.renderer;

namespace ThreeAPI.geometry
{
  public interface IShape
  {
    void Load(string filePath);
    void Save(string filePath);
    void ConfigureRenderer(IRenderer renderer);
  }
}