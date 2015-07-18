using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.renderer
{
  public interface IRenderer
  {
    void DrawTriangles(string name, int triangleCount);
    
    void AddObject(string objectName);
    void AddMatrixParameter(string name, Matrix4 matrix);
    void AddVectorParameter(string name, Vector3 vector);
    void AddNumberParameter(string name, float number);

    void LoadShaders(string shaderName);
    void UseShader(string shaderName);
    void SetVerticesData(List<Vector3> data);
    void SetFacesData(List<int> data);
    void SetVertexAttribute(int index, string name, int stride, int offset);
  }
}
