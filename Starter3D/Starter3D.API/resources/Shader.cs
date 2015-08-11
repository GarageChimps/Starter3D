using System;
using System.Collections.Generic;
using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public class Shader : IShader
  {
    private string _shaderName;
    private string _vertexShader;
    private string _fragmentShader;

    private readonly Dictionary<string, Vector3> _vectorParameters = new Dictionary<string, Vector3>();
    private readonly Dictionary<string, float> _numericParameters = new Dictionary<string, float>();
    private readonly Dictionary<string, ITexture> _textureParameters = new Dictionary<string, ITexture>();

    private bool _isDirty = true; 

    public string Name
    {
      get { return _shaderName; }
    }

    public void SetVectorParameter(string name, Vector3 vector)
    {
      if (!_vectorParameters.ContainsKey(name))
        throw new ApplicationException("Vector parameter doesnt exist in this shader");
      _vectorParameters[name] = vector;
      _isDirty = true;
    }

    public void SetNumericParameter(string name, float number)
    {
      if (!_numericParameters.ContainsKey(name))
        throw new ApplicationException("Numeric parameter doesnt exist in this shader");
      _numericParameters[name] = number;
      _isDirty = true;
    }

    public void SetTextureParameter(string name, ITexture texture)
    {
      if (!_textureParameters.ContainsKey(name))
        throw new ApplicationException("Texture doesnt exist in this shader");
      _textureParameters[name] = texture;
      _isDirty = true;
    }

    public void Configure(IRenderer renderer)
    {
      renderer.LoadShaders(_shaderName, _vertexShader, _fragmentShader);
    }

    public void Render(IRenderer renderer)
    {
      renderer.UseShader(_shaderName);
      foreach (var textureParameter in _textureParameters)
      {
        renderer.UseTexture(textureParameter.Value.Name, _shaderName, textureParameter.Key);
      }
      if (_isDirty)
      {
        foreach (var numericParameter in _numericParameters)
        {
          renderer.SetNumericParameter(numericParameter.Key, numericParameter.Value, _shaderName);
        }
        foreach (var vectorParameter in _vectorParameters)
        {
          renderer.SetVectorParameter(vectorParameter.Key, vectorParameter.Value, _shaderName);
        }
        _isDirty = false;
      }

    }

    public void Load(IDataNode dataNode, IResourceManager resourceManager)
    {
      _shaderName = dataNode.ReadParameter("key");
      _vertexShader = dataNode.ReadParameter("vertex");
      _fragmentShader = dataNode.ReadParameter("fragment");

      if (dataNode.HasParameter("numbers"))
      {
        var floats = dataNode.ReadParameterList("numbers");
        foreach (var f in floats)
        {
          _numericParameters.Add(f, default(float));
        }
      }

      if (dataNode.HasParameter("vectors"))
      {
        var vectors = dataNode.ReadParameterList("vectors");
        foreach (var v in vectors)
        {
          _vectorParameters.Add(v, default(Vector3));
        }
      }


      if (dataNode.HasParameter("textures"))
      {
        var textures = dataNode.ReadParameterList("textures");
        foreach (var t in textures)
        {
          _textureParameters.Add(t, null);
        }
      }

    }
  }
}