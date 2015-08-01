using System;
using Starter3D.Application.controllers;
using Starter3D.API.controller;
using Starter3D.API.utils;
using Starter3D.Renderers;

namespace Starter3D.Application.windows
{
  public class WindowFactory : IWindowFactory
  {
    private readonly IControllerFactory _controllerFactory;
    private readonly IRendererFactory _rendererFactory;

    public WindowFactory(IControllerFactory controllerFactory, IRendererFactory rendererFactory)
    {
      if (controllerFactory == null) throw new ArgumentNullException("controllerFactory");
      if (rendererFactory == null) throw new ArgumentNullException("rendererFactory");
      _controllerFactory = controllerFactory;
      _rendererFactory = rendererFactory;
    }

    public IWindow CreateWindow(int width, int height, IConfiguration configuration)
    {
      var windowType = (WindowType)Enum.Parse(typeof(WindowType), configuration.GetParameter("window"));
      var controllerType = (ControllerType)Enum.Parse(typeof(ControllerType), configuration.GetParameter("controller"));
      var rendererType = (RendererType)Enum.Parse(typeof(RendererType), configuration.GetParameter("renderer"));

      var renderer = _rendererFactory.CreateRenderer(rendererType);
      var controller = _controllerFactory.CreateController(controllerType, renderer);

      switch (windowType)
      {
        case WindowType.GlWindow:
          return new GLWindow(width, height, controller);
        case WindowType.WPFWindow:
          return new WPFWindow(width, height, controller);
        default:
          throw new ArgumentOutOfRangeException("windowType");
      }
      
    }
  }
}