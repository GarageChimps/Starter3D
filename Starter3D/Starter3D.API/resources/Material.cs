using System.Collections.Generic;
using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public class Material : IMaterial
  {
    private IShader _shader;
    private string _name;
    private readonly Dictionary<string, Vector3> _vectorParameters = new Dictionary<string, Vector3>();
    private readonly Dictionary<string, float> _numericParameters = new Dictionary<string, float>();
    private readonly Dictionary<string, ITexture> _textureParameters = new Dictionary<string, ITexture>();

    private bool _isDirty = true; 

    public string Name
    {
      get { return _name; }
    }

    public IShader Shader
    {
      get { return _shader; }
      set
      {
        _shader = value;
        _isDirty = true;
      }
    }

    public IEnumerable<KeyValuePair<string, float>> NumericParameters
    {
      get { return _numericParameters; }
    }

    public IEnumerable<KeyValuePair<string, Vector3>> VectorParameters
    {
      get { return _vectorParameters; }
    }

    public IEnumerable<KeyValuePair<string, ITexture>> Textures
    {
      get { return _textureParameters; }
    }

   
    public Material()
    {
      
    }

    public Material(IShader shader)
    {
      _shader = shader;
    }


    public virtual void Configure(IRenderer renderer)
    {
      foreach (var textureParameter in _textureParameters)
      {
        _shader.SetTextureParameter(textureParameter.Key, textureParameter.Value);
      } 
    }

    public void Render(IRenderer renderer)
    {
      if (_isDirty)
      {
        foreach (var numericParameter in _numericParameters)
        {
          _shader.SetNumericParameter(numericParameter.Key, numericParameter.Value);
        }
        foreach (var vectorParameter in _vectorParameters)
        {
          _shader.SetVectorParameter(vectorParameter.Key, vectorParameter.Value);
        }
        foreach (var textureParameter in _textureParameters)
        {
          _shader.SetTextureParameter(textureParameter.Key, textureParameter.Value);
        }
        _isDirty = false;
      }
      _shader.Render(renderer);
       
    }

    public virtual void Load(IDataNode dataNode, IResourceManager resourceManager)
    {
      var shaderName = dataNode.ReadParameter("shader");
      _shader = resourceManager.GetShader(shaderName);
      _name = dataNode.ReadParameter("key");
      var textureParameters = new Dictionary<string, string>();
      dataNode.ReadAllParameters(_vectorParameters, _numericParameters, textureParameters);
      foreach (var textureParameter in textureParameters)
      {
        if(resourceManager.HasTexture(textureParameter.Value))
          _textureParameters.Add(textureParameter.Key, resourceManager.GetTexture(textureParameter.Value));
      }
    }

    public void SetParameter(string name, float value)
    {
      if (_numericParameters.ContainsKey(name))
      {
        _numericParameters[name] = value;
        _isDirty = true;
      }
    }

    public void SetParameter(string name, Vector3 value)
    {
      if (_vectorParameters.ContainsKey(name))
      {
        _vectorParameters[name] = value;
        _isDirty = true;
      }
    }

    public void SetTexture(string name, ITexture texture)
    {
      if (_textureParameters.ContainsKey(name))
      {
        _textureParameters[name] = texture;
        _isDirty = true;
      }
    }


  }
}