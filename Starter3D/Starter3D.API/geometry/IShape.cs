using ThreeAPI.renderer;
using ThreeAPI.resources;

namespace ThreeAPI.geometry
{
  public interface IShape
  {
    IMaterial Material { get; set; }
    void Load(string filePath);
    void Save(string filePath);
    void ConfigureRenderer(IRenderer renderer);
  }
}