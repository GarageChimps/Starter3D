using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Starter3D.API.utils;

namespace Starter3D.API.resources
{
  public class ResourceManager : IResourceManager
  {
    private readonly Dictionary<string, IMaterial> _materials = new Dictionary<string, IMaterial>();
    private readonly IMaterialFactory _materialFactory;

    public ResourceManager(IMaterialFactory materialFactory)
    {
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
      foreach (var element in materialRoot.Elements())
      {
        var type = (MaterialType)Enum.Parse(typeof(MaterialType), element.Name.ToString());
        var key = element.Attribute("key").Value;
        var material = _materialFactory.CreateMaterial(type);
        material.Load(new XmlDataNode(element));
        AddMaterial(key, material);
      }
    }

    
    public IMaterial GetMaterial(string key)
    {
      if(!_materials.ContainsKey(key))
        throw new ApplicationException("Material key not found");
      return _materials[key];
    }

    public void AddMaterial(string key, IMaterial material)
    {
      _materials.Add(key, material);
    }

  }
}