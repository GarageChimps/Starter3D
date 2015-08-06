using Starter3D.API.resources;

namespace Starter3D.Plugin.PixelShader
{
  public class ShaderViewModel : ViewModelBase
  {
    private readonly IShader _shader;
    
    public IShader Shader
    {
      get { return _shader; }
    }

    public ShaderViewModel(IShader shader)
    {
      _shader = shader;
    }
  }
}