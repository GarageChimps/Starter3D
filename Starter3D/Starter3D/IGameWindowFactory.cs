using OpenTK.Platform;

namespace Starter3D
{
  public interface IGameWindowFactory
  {
    IGameWindow CreateGameWindow(int width, int height);
  }
}