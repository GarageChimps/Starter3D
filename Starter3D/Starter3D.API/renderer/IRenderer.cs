using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Starter3D.API.renderer
{
  public interface IRenderer
  {
    //Geometry object related methods    
    void LoadObject(string objectName);
    void DrawTriangles(string objectName, int triangleCount);
    void SetVerticesData(string objectName, List<Vector3> data);
    void SetFacesData(string objectName, List<int> data);
    void SetVertexAttribute(string objectName, string shaderName, int index, string vertexPropertyName, int stride, int offset);
    
    //Texture related methods
    void LoadTexture(string uniformName, string shader, int index, string textureName, Bitmap texture, TextureMinFilter minFilter, TextureMagFilter magFilter);
    void UseTexture(string textureName, string shader);

    //Shader related methods
    void LoadShaders(string shaderName, string vertexShader, string fragmentShader);
    void UseShader(string shaderName);
    void SetMatrixParameter(string name, Matrix4 matrix);
    void SetMatrixParameter(string name, Matrix4 matrix, string shader);
    void SetVectorParameter(string name, Vector3 vector);
    void SetVectorParameter(string name, Vector3 vector, string shader);
    void SetBooleanParameter(string name, bool value);
    void SetBooleanParameter(string name, bool value, string shader);
    void SetNumberParameter(string name, float number);
    void SetNumberParameter(string name, float number, string shader);

    //General rendering properties
    void SetBackgroundColor(Color4 color);
    void EnableZBuffer(bool enable);


  }
}
