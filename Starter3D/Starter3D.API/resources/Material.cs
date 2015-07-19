using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public class Material : IMaterial
  {
    private string _shaderName;
    private readonly Dictionary<string, Vector3> _vectorParameters = new Dictionary<string, Vector3>();
    private readonly Dictionary<string, float> _numericParameters = new Dictionary<string, float>();
    private readonly Dictionary<string, Bitmap> _textureParameters = new Dictionary<string, Bitmap>();
    
    public string ShaderName
    {
      get { return _shaderName; }
    }

   
    public Material()
    {
      
    }

    public Material(string shaderName)
    {
      _shaderName = shaderName;
    }


    public virtual void Configure(IRenderer renderer)
    {
      renderer.LoadShaders(_shaderName);
      foreach (var numericParameter in _numericParameters)
      {
        renderer.AddNumberParameter(numericParameter.Key, numericParameter.Value);
      }
      foreach (var vectorParameter in _vectorParameters)
      {
        renderer.AddVectorParameter(vectorParameter.Key, vectorParameter.Value);
      }
      int index = 0;
      foreach (var textureParameter in _textureParameters)
      {
        renderer.AddTexture(textureParameter.Key, index, textureParameter.Value);
        index++;
      }
      
    }

    public void UseMaterial(IRenderer renderer)
    {
      renderer.UseShader(_shaderName);
      foreach (var textureParameter in _textureParameters)
      {
        renderer.UseTexture(textureParameter.Key);
      }
    }

    public virtual void Load(IDataNode dataNode)
    {
      _shaderName = dataNode.ReadParameter("shader");
      dataNode.ReadAllParameters(_vectorParameters, _numericParameters, _textureParameters);
    }
  }
}