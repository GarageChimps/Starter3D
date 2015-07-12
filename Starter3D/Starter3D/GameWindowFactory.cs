using System;
using OpenTK.Platform;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;
using Starter3D.Examples;

namespace Starter3D
{
  public class GameWindowFactory : IGameWindowFactory
  {
    private readonly IRenderer _renderer;
    private readonly IResourceManager _resourceManager;
    private readonly ISceneNodeReader _reader;
    private readonly IConfiguration _configuration;

    public GameWindowFactory(IRenderer renderer, IResourceManager resourceManager, ISceneNodeReader reader, IConfiguration configuration)
    {
      if (renderer == null) throw new ArgumentNullException("renderer");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      if (reader == null) throw new ArgumentNullException("reader");
      if (configuration == null) throw new ArgumentNullException("configuration");
      _renderer = renderer;
      _resourceManager = resourceManager;
      _reader = reader;
      _configuration = configuration;
    }

    public IGameWindow CreateGameWindow(int width, int height)
    {
      return new SceneExampleWindow(width, height, _renderer, _reader, _resourceManager, _configuration);
    }
  }
}