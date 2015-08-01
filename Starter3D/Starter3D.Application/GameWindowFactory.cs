using System;
using OpenTK.Platform;
using Starter3D.API.controller;

namespace Starter3D.Application
{
  public class GameWindowFactory : IGameWindowFactory
  {
    private readonly IController _controller;

    public GameWindowFactory(IController controller)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      _controller = controller;
    }

    public IGameWindow CreateGameWindow(int width, int height)
    {
      return new GLWindow(width, height, _controller);
    }
  }
}