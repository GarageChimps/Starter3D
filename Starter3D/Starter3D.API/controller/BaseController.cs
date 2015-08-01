using System.Collections.Generic;
using System.Linq;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
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

    protected readonly ISceneNode _sceneGraph;
    protected readonly IEnumerable<ISceneNode> _sceneElements;
    protected readonly IEnumerable<ShapeNode> _objects;
    protected readonly IEnumerable<LightNode> _lights;
    protected readonly IEnumerable<CameraNode> _cameras;
    protected readonly IEnumerable<IMaterial> _materials;
    

    protected BaseController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager, IConfiguration configuration)
    {
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;
      _configuration = configuration;

      _resourceManager.Load(configuration.GetParameter("resources"));
      _materials = _resourceManager.GetMaterials();
      _sceneGraph = _sceneReader.Read(configuration.GetParameter("scene"));
      _sceneElements = _sceneGraph.GetNodes<ISceneNode>().ToList();
      _objects = _sceneGraph.GetNodes<ShapeNode>().ToList();
      _lights = _sceneGraph.GetNodes<LightNode>().ToList();
      _cameras = _sceneGraph.GetNodes<CameraNode>().ToList();
      
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