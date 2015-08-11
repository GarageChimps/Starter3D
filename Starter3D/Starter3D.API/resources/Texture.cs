using System;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public class Texture : ITexture
  {
    private string _name;
    private string _path;
    private Bitmap _image;
    private int _index;
    private TextureMinFilter _minFilter;
    private TextureMagFilter _magFilter;

    public string Name
    {
      get { return _name; }
    }

    public Bitmap Image
    {
      get { return _image; }
    }
    
    public void Configure(IRenderer renderer)
    {
      renderer.LoadTexture(_name, _index, _image, _minFilter, _magFilter);
    }

    public void Render(IRenderer renderer)
    {
      
    }

    public void Load(IDataNode dataNode, IResourceManager resourceManager)
    {
      _name = dataNode.ReadParameter("key");
      _path = dataNode.ReadParameter("path");
      _minFilter = TextureMinFilter.Linear;
      if (dataNode.HasParameter("minFilter"))
        _minFilter = (TextureMinFilter)Enum.Parse(typeof(TextureMinFilter), dataNode.ReadParameter("minFilter"));
      _magFilter = TextureMagFilter.Linear;
      if (dataNode.HasParameter("magFilter"))
        _magFilter = (TextureMagFilter)Enum.Parse(typeof(TextureMagFilter), dataNode.ReadParameter("magFilter"));
      _image = (Bitmap) System.Drawing.Image.FromFile(_path);
      _index = resourceManager.GetTextures().Count();
    }

    
  }
}