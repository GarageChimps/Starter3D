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
    private Window _window;
    private readonly IController _controller;
    private readonly IRenderer _renderer;
    private readonly RendererType _rendererType;

    public WPFWindow(IController controller, IRenderer renderer, RendererType rendererType)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      _controller = controller;
      _renderer = renderer;
      _rendererType = rendererType;
      
    }

    public void Run(double frameRate)
    {
      switch (_rendererType)
      {
        case RendererType.OpenGL:
          _window = new OpenGLWindow(_controller, frameRate);
          break;
        case RendererType.Direct3D:
          _window = new Direct3DWindow(_controller, _renderer, frameRate);
          break;
        case RendererType.Composite:
          _window = new CompositeWindow(_controller, _renderer, frameRate);
          break;
        default:
          throw new ApplicationException("Invalud rendering type");
      }
      Run();
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