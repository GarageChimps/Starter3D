using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.API.controller
{
  public abstract class BaseController : IController
  {
    protected readonly IRenderer _renderer;
    protected readonly ISceneReader _sceneReader;
    protected readonly IResourceManager _resourceManager;
    protected readonly IConfiguration _configuration;

    protected BaseController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager, IConfiguration configuration)
    {
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;
      _configuration = configuration;
    }

    public virtual void Load()
    {
      
    }

    public virtual void Render(double time)
    {
      
    }

    public virtual void Update(double time)
    {
      
    }

    public virtual void MouseDown(ControllerMouseButton button, int x, int y)
    {
     
    }

    public virtual void MouseUp(ControllerMouseButton button, int x, int y)
    {
      
    }

    public virtual void MouseWheel(int delta, int x, int y)
    {
      
    }

    public virtual void MouseMove(int x, int y, int deltaX, int deltaY)
    {
      
    }

    public virtual void KeyDown(int key)
    {
      
    }
  }
}