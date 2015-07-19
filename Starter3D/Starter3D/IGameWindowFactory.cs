using OpenTK.Platform;

namespace Starter3D.OpenGL
{
  public interface IGameWindowFactory
  {
    IGameWindow CreateGameWindow(int width, int height);
  }
}