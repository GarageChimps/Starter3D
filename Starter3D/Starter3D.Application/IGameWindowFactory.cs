using OpenTK.Platform;

namespace Starter3D.Application
{
  public interface IGameWindowFactory
  {
    IGameWindow CreateGameWindow(int width, int height);
  }
}