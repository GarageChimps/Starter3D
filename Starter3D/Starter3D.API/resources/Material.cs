using System.Collections.Generic;
using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public class Material : IMaterial
  {
    private string _shaderName;
    private Dictionary<string, Vector3> _vectorParameters = new Dictionary<string, Vector3>();
    private Dictionary<string, float> _numericParameters = new Dictionary<string, float>();
    private Dictionary<string, bool> _booleanParameters = new Dictionary<string, bool>();
    
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
      renderer.UseShader(_shaderName);
      foreach (var numericParameter in _numericParameters)
      {
        renderer.AddNumberParameter(numericParameter.Key, numericParameter.Value);
      }
      foreach (var vectorParameter in _vectorParameters)
      {
        renderer.AddVectorParameter(vectorParameter.Key, vectorParameter.Value);
      }
      foreach (var booleanParameter in _booleanParameters)
      {
        renderer.AddBooleanParameter(booleanParameter.Key, booleanParameter.Value);
      }
      
    }

    public void UseMaterial(IRenderer renderer)
    {
      renderer.UseShader(_shaderName);
    }

    public virtual void Load(IDataNode dataNode)
    {
      _shaderName = dataNode.ReadParameter("shader");
      dataNode.ReadAllParameters(_vectorParameters, _numericParameters, _booleanParameters);
    }
  }
}