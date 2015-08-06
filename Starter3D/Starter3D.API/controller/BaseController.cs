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

    protected ISceneNode _sceneGraph;
    protected IEnumerable<ISceneNode> _sceneElements;
    protected IEnumerable<ShapeNode> _objects;
    protected IEnumerable<LightNode> _lights;
    protected IEnumerable<CameraNode> _cameras;
    protected IEnumerable<IMaterial> _materials;

    protected abstract string ScenePath { get; }
    protected abstract string ResourcePath { get; } 


    protected BaseController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
    {
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;

      _resourceManager.Load(ResourcePath);
      _materials = _resourceManager.GetMaterials();
      _sceneGraph = _sceneReader.Read(ScenePath);
      _sceneElements = _sceneGraph.GetNodes<ISceneNode>().ToList();
      _objects = _sceneGraph.GetNodes<ShapeNode>().ToList();
      _lights = _sceneGraph.GetNodes<LightNode>().ToList();
      _cameras = _sceneGraph.GetNodes<CameraNode>().ToList();
    }

    protected void Init(string scenePath = "", string resourcesPath = "")
    {
      
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

    public virtual void UpdateSize(double width, double height)
    {
      
    }
  }
}