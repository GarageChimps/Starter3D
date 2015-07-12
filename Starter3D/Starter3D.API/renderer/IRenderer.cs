using System.Collections.Generic;
using OpenTK;
using ThreeAPI.geometry;

namespace ThreeAPI.renderer
{
  public interface IRenderer
  {
    void Render(IMesh mesh);

    void SetShaders(string vertexShaderPath, string fragmentShaderPath);
    void AddMesh(IMesh mesh);
    void SetVerticesData(List<Vector3> data);
    void SetFacesData(List<int> data);
    void SetVertexAttribute(int index, string name, int stride, int offset);
  }
}
