using Starter3D.API.utils;

namespace Starter3D.Application.windows
{
  public interface IWindowFactory
  {
    IWindow CreateWindow(IConfiguration configuration);
  }
}