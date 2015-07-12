using System.Collections.Generic;
using OpenTK;
using Starter3D.API.geometry;

namespace Starter3D.API.renderer
{
  public interface IRenderer
  {
    void Render(IMesh mesh);
    
    void AddMesh(IMesh mesh);
    void AddMatrixParameter(string name, Matrix4 matrix);
    void AddVectorParameter(string name, Vector3 vector);
    void AddNumberParameter(string name, float number);

    void SetShaders(string vertexShaderPath, string fragmentShaderPath);
    void SetVerticesData(List<Vector3> data);
    void SetFacesData(List<int> data);
    void SetVertexAttribute(int index, string name, int stride, int offset);
  }
}
