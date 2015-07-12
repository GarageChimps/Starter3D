using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ThreeAPI.geometry;
using ThreeAPI.geometry.factories;

namespace ThreeAPI.resources
{
  public class ResourceManager : IResourceManager
  {
    private readonly Dictionary<string, IMaterial> _materials = new Dictionary<string, IMaterial>();
    private readonly Dictionary<string, IShape> _shapes = new Dictionary<string, IShape>();
    private readonly Dictionary<string, string> _shaders = new Dictionary<string, string>();

    private readonly IShapeFactory _shapeFactory;
    private readonly IMaterialFactory _materialFactory;

    public ResourceManager(IMaterialFactory materialFactory)
    {
      //if (shapeFactory == null) throw new ArgumentNullException("shapeFactory");
      if (materialFactory == null) throw new ArgumentNullException("materialFactory");
      _materialFactory = new MaterialFactory();
    }

    public void Load(string resourceFile)
    {
      var xmlDoc = XDocument.Load(resourceFile);
      var element = xmlDoc.Elements("Resources").First();
      LoadMaterials(element.Elements("Materials").First());
    }

    private void LoadMaterials(XElement materialRoot)
    {
      foreach (var element in materialRoot.Elements("Material"))
      {
        var key = element.Attribute("key").Value;
        var vertexShader = element.Attribute("vertexShader").Value;
        var fragmentShader = element.Attribute("fragmentShader").Value;
        var material = _materialFactory.CreateMaterial(vertexShader, fragmentShader);
        AddMaterial(key, material);
      }
    }

    
    public IMaterial GetMaterial(string key)
    {
      if(!_materials.ContainsKey(key))
        throw new ApplicationException("Material key not found");
      return _materials[key];
    }

    public IShape GetShape(string key)
    {
      if (!_shapes.ContainsKey(key))
        throw new ApplicationException("Shape key not found");
      return _shapes[key];
    }

    public string GetShader(string key)
    {
      if (!_shaders.ContainsKey(key))
        throw new ApplicationException("Shader key not found");
      return _shaders[key];
    }

    public void AddMaterial(string key, IMaterial material)
    {
      _materials.Add(key, material);
    }

    public void AddShape(string key, IShape shape)
    {
      _shapes.Add(key, shape);
    }

    public void AddShader(string key, string shader)
    {
      _shaders.Add(key, shader);
    }
  }
}