using System;
using OpenTK.Platform;
using ThreeAPI.examples;
using ThreeAPI.renderer;
using ThreeAPI.resources;
using ThreeAPI.scene.persistence;

namespace ThreeDU
{
  public class GameWindowFactory : IGameWindowFactory
  {
    private readonly IRenderer _renderer;
    private readonly IResourceManager _resourceManager;
    private readonly ISceneNodeReader _reader;

    public GameWindowFactory(IRenderer renderer, IResourceManager resourceManager, ISceneNodeReader reader)
    {
      if (renderer == null) throw new ArgumentNullException("renderer");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      if (reader == null) throw new ArgumentNullException("reader");
      _renderer = renderer;
      _resourceManager = resourceManager;
      _reader = reader;
    }

    public IGameWindow CreateGameWindow(int width, int height)
    {
      return new PlainWindow(width, height, _renderer, _reader, _resourceManager);
    }
  }
}