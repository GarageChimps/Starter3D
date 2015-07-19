using OpenTK;
using Starter3D.API.renderer;
using Starter3D.API.resources;

namespace Starter3D.API.geometry
{
  public interface IShape
  {
    IMaterial Material { get; set; }
    void Load(string filePath);
    void Save(string filePath);
    void Configure(IRenderer renderer);
    void Render(IRenderer renderer, Matrix4 modelTransform);
  }
}