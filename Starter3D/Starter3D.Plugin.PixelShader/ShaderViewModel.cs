using Starter3D.API.resources;

namespace Starter3D.Plugin.PixelShader
{
  public class ShaderViewModel : ViewModelBase
  {
    private readonly IShader _shader;
    private string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public IShader Shader
    {
      get { return _shader; }
    }

    public ShaderViewModel(IShader shader)
    {
      _shader = shader;
      _name = shader.Name;
    }
  }
}