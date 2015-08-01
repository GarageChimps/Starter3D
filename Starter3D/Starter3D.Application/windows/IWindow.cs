using System;

namespace Starter3D.Application.windows
{
  public interface IWindow : IDisposable
  {
    void Run(double frameRate);
  }
}