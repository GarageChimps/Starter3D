using System;
using System.Windows;
using Starter3D.Application.ui;
using Starter3D.API.controller;

namespace Starter3D.Application.windows
{
  public class WPFWindow : System.Windows.Application, IWindow
  {
    private readonly OpenGLWindow _window;
    private readonly IController _controller;

    public WPFWindow(IController controller)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      _controller = controller;
      _window = new OpenGLWindow(_controller);

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