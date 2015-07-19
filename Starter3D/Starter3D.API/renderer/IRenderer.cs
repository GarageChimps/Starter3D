using System.Collections.Generic;
using System.Drawing;
using OpenTK;

namespace Starter3D.API.renderer
{
  public interface IRenderer
  {
    void DrawTriangles(string objectName, int triangleCount);
    
    void AddObject(string objectName);
    void BeginUsingObject(string objectName);
    void StopUsingObject();
    void SetVerticesData(string objectName, List<Vector3> data);
    void SetFacesData(string objectName, List<int> data);
    void SetVertexAttribute(string objectName, string shaderName, int index, string vertexPropertyName, int stride, int offset);

    void AddMatrixParameter(string name, Matrix4 matrix);
    void AddVectorParameter(string name, Vector3 vector);
    void AddBooleanParameter(string name, bool value);
    void AddNumberParameter(string name, float number);

    void AddTexture(string textureName, int index, Bitmap texture);
    void UseTexture(string textureName);

    void LoadShaders(string shaderName);
    void UseShader(string shaderName);

    
  }
}
