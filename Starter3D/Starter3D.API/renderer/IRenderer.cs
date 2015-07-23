﻿using System.Collections.Generic;
using System.Drawing;
using OpenTK;

namespace Starter3D.API.renderer
{
  public interface IRenderer
  {
        
    void LoadObject(string objectName);
    void DrawTriangles(string objectName, int triangleCount);
    void SetVerticesData(string objectName, List<Vector3> data);
    void SetFacesData(string objectName, List<int> data);
    void SetVertexAttribute(string objectName, string shaderName, int index, string vertexPropertyName, int stride, int offset);
    
    void LoadTexture(string textureName, string shader, int index, Bitmap texture);
    void UseTexture(string textureName, string shader);

    void LoadShaders(string shaderName);
    void UseShader(string shaderName);
    void SetMatrixParameter(string name, Matrix4 matrix);
    void SetMatrixParameter(string name, Matrix4 matrix, string shader);
    void SetVectorParameter(string name, Vector3 vector);
    void SetVectorParameter(string name, Vector3 vector, string shader);
    void SetBooleanParameter(string name, bool value);
    void SetBooleanParameter(string name, bool value, string shader);
    void SetNumberParameter(string name, float number);
    void SetNumberParameter(string name, float number, string shader);

    
  }
}
