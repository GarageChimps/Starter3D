using System;
using System.Windows;
using Starter3D.Application.ui;
using Starter3D.API.controller;
using Starter3D.API.ui;

namespace Starter3D.Application.windows
{
  public class WPFWindow : System.Windows.Application, IWindow
  {
    private readonly OpenGLWindow _window;
    private readonly IController _controller;
    private readonly IUserInterface _userInterface;

    public WPFWindow(int width, int height, IController controller, IUserInterface userInterface)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      if (userInterface == null) throw new ArgumentNullException("userInterface");
      _controller = controller;
      _userInterface = userInterface;
      _window = new OpenGLWindow(width, height, _controller, _userInterface);

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