using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Starter3D.API.renderer;
using Starter3D.API.utils;

namespace Starter3D.API.resources
{
  public class ResourceManager : IResourceManager
  {
    private readonly Dictionary<string, IShader> _shaders = new Dictionary<string, IShader>();
    private readonly Dictionary<string, IMaterial> _materials = new Dictionary<string, IMaterial>();
    private readonly Dictionary<string, ITexture> _textures = new Dictionary<string, ITexture>();
    private readonly List<IResource> _resources = new List<IResource>(); 
    private readonly IMaterialFactory _materialFactory;
    private readonly IShaderFactory _shaderFactory;
    private readonly ITextureFactory _textureFactory;

    public ResourceManager(IMaterialFactory materialFactory, IShaderFactory shaderFactory, ITextureFactory textureFactory)
    {
      if (materialFactory == null) throw new ArgumentNullException("materialFactory");
      if (shaderFactory == null) throw new ArgumentNullException("shaderFactory");
      if (textureFactory == null) throw new ArgumentNullException("textureFactory");
      _materialFactory = new MaterialFactory();
      _shaderFactory = shaderFactory;
      _textureFactory = textureFactory;
    }

    public void Load(string resourceFile)
    {
      if (!File.Exists(resourceFile))
        return;
      var xmlDoc = XDocument.Load(resourceFile);
      var element = xmlDoc.Elements("Resources").First();
      if (element.Elements("Shaders").Any())
        LoadShaders(element.Elements("Shaders").First());
      if (element.Elements("Textures").Any())
        LoadTextures(element.Elements("Textures").First());
      if (element.Elements("Materials").Any())
        LoadMaterials(element.Elements("Materials").First());
    }

    public void Configure(IRenderer renderer)
    {
      foreach (var resource in _resources)
      {
        resource.Configure(renderer);
      }
    }

    private void LoadTextures(XElement texturesRoot)
    {
      foreach (var element in texturesRoot.Elements())
      {
        var type = (TextureType)Enum.Parse(typeof(TextureType), element.Name.ToString());
        var key = element.Attribute("key").Value;
        var texture = _textureFactory.CreateTexture(type);
        texture.Load(new XmlDataNode(element), this);
        AddTexture(key, texture);
      }
    }

    private void LoadShaders(XElement shadersRoot)
    {
      foreach (var element in shadersRoot.Elements())
      {
        var type = (ShaderResourceType)Enum.Parse(typeof(ShaderResourceType), element.Name.ToString());
        var key = element.Attribute("key").Value;
        var shader = _shaderFactory.CreateShader(type);
        shader.Load(new XmlDataNode(element), this);
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
      _resources.Add(material);
    }

    public bool HasMaterial(string key)
    {
      return _materials.ContainsKey(key);
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
      _resources.Add(shader);
    }

    public IEnumerable<IShader> GetShaders()
    {
      return _shaders.Values;
    }

    public bool HasShader(string key)
    {
      return _shaders.ContainsKey(key);
    }

    public ITexture GetTexture(string key)
    {
      if (!_textures.ContainsKey(key))
        throw new ApplicationException("Texture key not found");
      return _textures[key];
    }

    public void AddTexture(string key, ITexture texture)
    {
      _textures.Add(key, texture);
      _resources.Add(texture);
    }

    public IEnumerable<ITexture> GetTextures()
    {
      return _textures.Values;
    }

    public bool HasTexture(string key)
    {
      return _textures.ContainsKey(key);
    }
  }
}