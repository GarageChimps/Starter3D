using System;
using System.Windows;
using Starter3D.Application.ui;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.Renderers;

namespace Starter3D.Application.windows
{
  public class WPFWindow : System.Windows.Application, IWindow
  {
    private readonly Window _window;
    private readonly IController _controller;

    public WPFWindow(IController controller, IRenderer renderer, RendererType rendererType)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      _controller = controller;
      if(rendererType == RendererType.OpenGL)
        _window = new OpenGLWindow(_controller);
      if (rendererType == RendererType.Direct3D)
        _window = new Direct3DWindow(_controller, renderer);

    }

    public void Run(double frameRate)
    {
      this.Run();
    }

    public void Dispose()
    {
      
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      _window.Show();
    }
  }
}