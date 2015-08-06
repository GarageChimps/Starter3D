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
    private readonly IConfiguration _configuration;

    public WindowFactory(IRendererFactory rendererFactory, IResourceManager resourceManager, ISceneReader sceneReader, IConfiguration configuration)
    {
      if (rendererFactory == null) throw new ArgumentNullException("rendererFactory");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      if (sceneReader == null) throw new ArgumentNullException("sceneReader");
      if (configuration == null) throw new ArgumentNullException("configuration");
      _rendererFactory = rendererFactory;
      _resourceManager = resourceManager;
      _sceneReader = sceneReader;
      _configuration = configuration;
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
      var userInterfaceType = assembly.GetTypes().First(m => m.IsClass && m.GetInterface("IUserInterface") != null);

      var parameter = new object[4];
      parameter[0] = renderer;
      parameter[1] = _sceneReader;
      parameter[2] = _resourceManager;
      parameter[3] = _configuration;

      var controller = (IController)Activator.CreateInstance(controllerType, parameter);
      var userInterface = (IUserInterface) Activator.CreateInstance(userInterfaceType);

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