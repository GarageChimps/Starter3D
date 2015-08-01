using System;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.Application.controllers
{
  public class ControllerFactory : IControllerFactory
  {
    private readonly IResourceManager _resourceManager;
    private readonly ISceneReader _sceneReader;
    private readonly IConfiguration _configuration;

    public ControllerFactory(IResourceManager resourceManager, ISceneReader sceneReader, IConfiguration configuration)
    {
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      if (sceneReader == null) throw new ArgumentNullException("sceneReader");
      if (configuration == null) throw new ArgumentNullException("configuration");
      _resourceManager = resourceManager;
      _sceneReader = sceneReader;
      _configuration = configuration;
    }

    public IController CreateController(ControllerType type, IRenderer renderer)
    {
      switch (type)
      {
        case ControllerType.MaterialEditor:
          return new MaterialEditorController(renderer, _sceneReader, _resourceManager, _configuration);
        case ControllerType.PixelShader:
          throw new NotImplementedException();
        default:
          throw new ArgumentOutOfRangeException("type");
      }
    }
  }
}