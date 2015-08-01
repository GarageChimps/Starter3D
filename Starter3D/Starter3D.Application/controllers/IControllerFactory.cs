using Starter3D.API.controller;
using Starter3D.API.renderer;

namespace Starter3D.Application.controllers
{
  public interface IControllerFactory
  {
    IController CreateController(ControllerType type, IRenderer renderer);
  }
}