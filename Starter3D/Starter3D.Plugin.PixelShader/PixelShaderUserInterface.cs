using Starter3D.API.ui;

namespace Starter3D.Plugin.PixelShader
{
  public class PixelShaderUserInterface : IUserInterface
  {
    private readonly PixelShaderView _view;

    public object View
    {
      get { return _view; }
    }

    public PixelShaderUserInterface()
    {
      _view = new PixelShaderView(); 
    }

  }
}