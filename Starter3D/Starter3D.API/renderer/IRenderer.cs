using System.Collections.Generic;
using System.Drawing;
using OpenTK;

namespace Starter3D.API.renderer
{
  public interface IRenderer
  {
    void DrawTriangles(string objectName, int triangleCount);
    
    void AddObject(string objectName);
    void SetVerticesData(string objectName, List<Vector3> data);
    void SetFacesData(string objectName, List<int> data);
    void SetVertexAttribute(string objectName, string shaderName, int index, string vertexPropertyName, int stride, int offset);

    void AddMatrixParameter(string name, Matrix4 matrix);
    void AddMatrixParameter(string name, Matrix4 matrix, string shader);
    void AddVectorParameter(string name, Vector3 vector);
    void AddVectorParameter(string name, Vector3 vector, string shader);
    void AddBooleanParameter(string name, bool value);
    void AddBooleanParameter(string name, bool value, string shader);
    void AddNumberParameter(string name, float number);
    void AddNumberParameter(string name, float number, string shader);

    //void AddTexture(string textureName, int index, Bitmap texture);
    void LoadTexture(string textureName, string shader, int index, Bitmap texture);
    void UseTexture(string textureName, string shader);

    void LoadShaders(string shaderName);
    void UseShader(string shaderName);

    
  }
}
