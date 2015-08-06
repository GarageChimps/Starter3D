using System.Collections.Generic;
using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public class Material : IMaterial
  {
    private IShader _shader;
    private readonly Dictionary<string, Vector3> _vectorParameters = new Dictionary<string, Vector3>();
    private readonly Dictionary<string, float> _numericParameters = new Dictionary<string, float>();
    private readonly Dictionary<string, ITexture> _textureParameters = new Dictionary<string, ITexture>();

    public IShader Shader
    {
      get { return _shader; }
      set { _shader = value; }
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
      _shader.Configure(renderer);
    }

    public void Render(IRenderer renderer)
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
      _shader.Render(renderer);
    }

    public virtual void Load(IDataNode dataNode, IResourceManager resourceManager)
    {
      var shaderName = dataNode.ReadParameter("shader");
      _shader = resourceManager.GetShader(shaderName);
      var textureParameters = new Dictionary<string, string>();
      dataNode.ReadAllParameters(_vectorParameters, _numericParameters, textureParameters);
      foreach (var textureParameter in textureParameters)
      {
        if(resourceManager.HasTexture(textureParameter.Value))
          _textureParameters.Add(textureParameter.Key, resourceManager.GetTexture(textureParameter.Value));
      }
    }
  }
}