using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using TextureMagFilter = OpenTK.Graphics.OpenGL.TextureMagFilter;
using TextureMinFilter = OpenTK.Graphics.OpenGL.TextureMinFilter;
using Starter3D.API.renderer;
using Starter3D.API.utils;

namespace Starter3D.Renderers
{
  public class CompositeRenderer : IRenderer
  {
    private readonly List<IRenderer> _renderers = new List<IRenderer>();
    private readonly OpenGLRenderer _glRenderer;
    private readonly Direct3DRenderer _d3dRenderer;

    public CompositeRenderer()
    {
      _glRenderer = new OpenGLRenderer();
      _d3dRenderer = new Direct3DRenderer();
      _renderers.Add(_glRenderer);
      _renderers.Add(_d3dRenderer);
    }

    public Direct3DRenderer D3DRenderer
    {
      get { return _d3dRenderer; }
    }

    public OpenGLRenderer GlRenderer
    {
      get { return _glRenderer; }
    }

    public void LoadObject(string objectName)
    {
      foreach (var renderer in _renderers)
      {
        renderer.LoadObject(objectName);
      }
    }

    public void DrawTriangles(string objectName, int triangleCount)
    {
      foreach (var renderer in _renderers)
      {
        renderer.DrawTriangles(objectName, triangleCount);
      }
    }

    public void SetFacesData(string objectName, List<int> indices)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetFacesData(objectName, indices);
      }
    }

    public void SetVertexAttribute(string objectName, string shaderName, int index, string vertexPropertyName, int stride,
      int offset)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetVertexAttribute(objectName, shaderName, index, vertexPropertyName, stride, offset);
      }
    }

    public void LoadTexture(string textureName, int index, Bitmap texture, TextureMinFilter minFilter, TextureMagFilter magFilter)
    {
      foreach (var renderer in _renderers)
      {
        renderer.LoadTexture(textureName, index, texture, minFilter, magFilter);
      }
    }

    public void UseTexture(string textureName, string shader, string uniformShader)
    {
      foreach (var renderer in _renderers)
      {
        renderer.UseTexture(textureName, shader, uniformShader);
      }
    }

    public void LoadShaders(string shaderName, string vertexShader, string fragmentShader)
    {
      foreach (var renderer in _renderers)
      {
        renderer.LoadShaders(shaderName, vertexShader, fragmentShader);
      }
    }

    public void UseShader(string shaderName)
    {
      foreach (var renderer in _renderers)
      {
        renderer.UseShader(shaderName);
      }
    }

    public void SetNumericParameter(string name, float number)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetNumericParameter(name, number);
      }
    }

    public void SetNumericParameter(string name, float number, string shader)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetNumericParameter(name, number, shader);
      }
    }

    public void EnableZBuffer(bool enable)
    {
      foreach (var renderer in _renderers)
      {
        renderer.EnableZBuffer(enable);
      }
    }

    public void EnableWireframe(bool enable)
    {
      foreach (var renderer in _renderers)
      {
        renderer.EnableWireframe(enable);
      }
    }

    public void SetCullMode(CullMode cullMode)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetCullMode(cullMode);
      }
    }

    public void SetBackgroundColor(float r, float g, float b)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetBackgroundColor(r,g,b);
      }
    }

    public void SetVectorArrayParameter(string name, int index, Vector3 vector, string shader)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetVectorArrayParameter(name, index, vector, shader);
      }
    }

    public void SetVectorArrayParameter(string name, int index, Vector3 vector)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetVectorArrayParameter(name, index, vector);
      }
    }

    public void SetVectorParameter(string name, Vector3 vector, string shader)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetVectorParameter(name, vector, shader);
      }
    }

    public void SetVectorParameter(string name, Vector3 vector)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetVectorParameter(name, vector);
      }
    }

    public void SetMatrixParameter(string name, Matrix4 matrix, string shader)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetMatrixParameter(name, matrix, shader);
      }
    }

    public void SetMatrixParameter(string name, Matrix4 matrix)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetMatrixParameter(name, matrix);
      }
    }

   

    public void SetVerticesData(string objectName, List<Vector3> data)
    {
      foreach (var renderer in _renderers)
      {
        renderer.SetVerticesData(objectName, data);
      }
    }
  }
}