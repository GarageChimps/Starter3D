using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Starter3D.API.controller;
using Starter3D.API.resources;
using Starter3D.API.scene.persistence;
using Starter3D.API.ui;
using Starter3D.API.utils;
using Starter3D.Renderers;

namespace Starter3D.Application.windows
{
  public class WindowFactory : IWindowFactory
  {
    private readonly IRendererFactory _rendererFactory;
    private readonly IResourceManager _resourceManager;
    private readonly ISceneReader _sceneReader;

    public WindowFactory(IRendererFactory rendererFactory, IResourceManager resourceManager, ISceneReader sceneReader)
    {
      if (rendererFactory == null) throw new ArgumentNullException("rendererFactory");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      if (sceneReader == null) throw new ArgumentNullException("sceneReader");
      _rendererFactory = rendererFactory;
      _resourceManager = resourceManager;
      _sceneReader = sceneReader;
    }

    public IWindow CreateWindow(int width, int height, IConfiguration configuration)
    {
      
      var windowType = (WindowType)Enum.Parse(typeof(WindowType), configuration.GetParameter("window"));
      var rendererType = (RendererType)Enum.Parse(typeof(RendererType), configuration.GetParameter("renderer"));
      var renderer = _rendererFactory.CreateRenderer(rendererType);
      
      var pluginFile = configuration.GetParameter("plugin") + ".dll";
      var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pluginFile);
      var assembly = Assembly.LoadFile(pluginPath);
      var controllerType = assembly.GetTypes().First(m => m.IsClass && m.GetInterface("IController") != null);
      var userInterfaceType = assembly.GetTypes().FirstOrDefault(m => m.IsClass && m.GetInterface("IUserInterface") != null);

      var controllerParameters = new object[3];
      controllerParameters[0] = renderer;
      controllerParameters[1] = _sceneReader;
      controllerParameters[2] = _resourceManager;

      var controller = (IController)Activator.CreateInstance(controllerType, controllerParameters);

      var userInterfaceParameters = new object[1];
      userInterfaceParameters[0] = controller;
      IUserInterface userInterface = null;
      if(userInterfaceType != null)
        userInterface = (IUserInterface)Activator.CreateInstance(userInterfaceType, userInterfaceParameters);

      switch (windowType)
      {
        case WindowType.GlWindow:
          return new GLWindow(width, height, controller);
        case WindowType.WPFWindow:
          return new WPFWindow(width, height, controller, userInterface);
        default:
          throw new ArgumentOutOfRangeException("windowType");
      }
      
    }
  }
}