using System.Collections.Generic;
using OpenTK;
using ThreeAPI.renderer;

namespace ThreeAPI.geometry
{
  public interface IVertex
  {
    Vector3 Position { get; }
    Vector3 Normal { get; set; }
    Vector3 TextureCoords { get; }
    bool HasValidNormal();
    void AppendData(List<Vector3> vertexData);
    void ConfigureRenderer(IRenderer renderer);
  }
}