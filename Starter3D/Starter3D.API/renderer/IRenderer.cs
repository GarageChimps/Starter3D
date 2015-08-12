using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Starter3D.API.utils;

namespace Starter3D.API.renderer
{
  /// <summary>
  /// Interface for a generic renderer that handles geometry (objects), textures, shaders and shader parameters
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IRenderer
  {
    //Geometry object related methods    
    void LoadObject(string objectName);
    void DrawTriangles(string objectName, int triangleCount);
    void SetVerticesData(string objectName, List<Vector3> data);
    void SetFacesData(string objectName, List<int> indices);
    void SetVertexAttribute(string objectName, string shaderName, int index, string vertexPropertyName, int stride, int offset);
    
    //Texture related methods
    void LoadTexture(string textureName, int index, Bitmap texture, TextureMinFilter minFilter, TextureMagFilter magFilter);
    void UseTexture(string textureName, string shader, string uniformName);

    //Shader related methods
    void LoadShaders(string shaderName, string vertexShader, string fragmentShader);
    void UseShader(string shaderName);
    void SetMatrixParameter(string name, Matrix4 matrix);
    void SetMatrixParameter(string name, Matrix4 matrix, string shader);
    void SetVectorParameter(string name, Vector3 vector);
    void SetVectorParameter(string name, Vector3 vector, string shader);
    void SetVectorArrayParameter(string name, int index, Vector3 vector);
    void SetVectorArrayParameter(string name, int index, Vector3 vector, string shader);
    void SetNumericParameter(string name, float number);
    void SetNumericParameter(string name, float number, string shader);

    //General rendering properties
    void SetBackgroundColor(float r, float g, float b);
    void EnableZBuffer(bool enable);
    void EnableWireframe(bool enable);
    void SetCullMode(CullMode cullMode);

  }
}
