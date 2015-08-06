using Starter3D.API.ui;

namespace Starter3D.Plugin.PixelShader
{
  public class PixelShaderUserInterface : IUserInterface
  {
    private readonly PixelShaderController _controller;
    private readonly PixelShaderView _view;

    public object View
    {
      get { return _view; }
    }

    public PixelShaderUserInterface(PixelShaderController controller)
    {
      _controller = controller;
      _view = new PixelShaderView();
      _view.DataContext = this;
    }
  }
}