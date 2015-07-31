using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Starter3D.API.utils;

namespace Starter3D.API.resources
{
  public class ResourceManager : IResourceManager
  {
    private readonly Dictionary<string, IShader> _shaders = new Dictionary<string, IShader>();
    private readonly Dictionary<string, IMaterial> _materials = new Dictionary<string, IMaterial>();
    private readonly IMaterialFactory _materialFactory;
    private readonly IShaderFactory _shaderFactory;

    public ResourceManager(IMaterialFactory materialFactory, IShaderFactory shaderFactory)
    {
      if (materialFactory == null) throw new ArgumentNullException("materialFactory");
      if (shaderFactory == null) throw new ArgumentNullException("shaderFactory");
      _materialFactory = new MaterialFactory();
      _shaderFactory = shaderFactory;
    }

    public void Load(string resourceFile)
    {
      var xmlDoc = XDocument.Load(resourceFile);
      var element = xmlDoc.Elements("Resources").First();
      LoadShaders(element.Elements("Shaders").First());
      LoadMaterials(element.Elements("Materials").First());
    }

    private void LoadShaders(XElement shadersRoot)
    {
      foreach (var element in shadersRoot.Elements())
      {
        var type = (ShaderResourceType)Enum.Parse(typeof(ShaderResourceType), element.Name.ToString());
        var key = element.Attribute("key").Value;
        var shader = _shaderFactory.CreateShader(type);
        shader.Load(new XmlDataNode(element));
        AddShader(key, shader);
      }
    }

    private void LoadMaterials(XElement materialRoot)
    {
      foreach (var element in materialRoot.Elements())
      {
        var type = (MaterialType)Enum.Parse(typeof(MaterialType), element.Name.ToString());
        var key = element.Attribute("key").Value;
        var material = _materialFactory.CreateMaterial(type);
        material.Load(new XmlDataNode(element), this);
        AddMaterial(key, material);
        
      }
    }

    
    public IMaterial GetMaterial(string key)
    {
      if(!_materials.ContainsKey(key))
        throw new ApplicationException("Material key not found");
      return _materials[key];
    }

    public IEnumerable<IMaterial> GetMaterials()
    {
      return _materials.Values;
    }

    public void AddMaterial(string key, IMaterial material)
    {
      _materials.Add(key, material);
    }

    public IShader GetShader(string key)
    {
      if(!_shaders.ContainsKey(key))
        throw new ApplicationException("Shader key not found");
      return _shaders[key];
    }

    public void AddShader(string key, IShader shader)
    {
      _shaders.Add(key, shader);
    }
  }
}