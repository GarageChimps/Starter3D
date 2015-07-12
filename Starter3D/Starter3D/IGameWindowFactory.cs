using OpenTK.Platform;

namespace ThreeDU
{
  public interface IGameWindowFactory
  {
    IGameWindow CreateGameWindow(int width, int height);
  }
}