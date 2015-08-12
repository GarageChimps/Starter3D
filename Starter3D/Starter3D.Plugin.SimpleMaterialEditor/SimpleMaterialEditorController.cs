using System;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.Plugin.SimpleMaterialEditor
{
  public class SimpleMaterialEditorController : IController
  {
    private const string ScenePath = @"scenes/simpleScene.xml";
    private const string ResourcePath = @"resources/simpleResources.xml";

    private readonly IRenderer _renderer;
    private readonly ISceneReader _sceneReader;
    private readonly IResourceManager _resourceManager;

    private readonly IScene _scene;
    
    private readonly SimpleMaterialEditorView _centralView;
    
    
    public int Width
    {
      get { return 800; }
    }

    public int Height
    {
      get { return 600; }
    }

    public bool IsFullScreen
    {
      get { return true; }
    }

    public object CentralView
    {
      get { return _centralView; }
    }

    public object LeftView
    {
      get { return null; }
    }

    public object RightView
    {
      get { return null; }
    }

    public object TopView
    {
      get { return null; }
    }

    public object BottomView
    {
      get { return null; }
    }

    public bool HasUserInterface
    {
      get { return true; }
    }

    public string Name
    {
      get { return "Simple Material Editor"; }
    }

    public SimpleMaterialEditorController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
    {
      if (renderer == null) throw new ArgumentNullException("renderer");
      if (sceneReader == null) throw new ArgumentNullException("sceneReader");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;

      _resourceManager.Load(ResourcePath);
      _scene = _sceneReader.Read(ScenePath);
      
      _centralView = new SimpleMaterialEditorView();
    }
    
    public void Load()
    {
      InitRenderer();

      _resourceManager.Configure(_renderer);
      _scene.Configure(_renderer);

    }

    private void InitRenderer()
    {
      _renderer.SetBackgroundColor(0.9f,0.9f,1.0f);
      _renderer.EnableZBuffer(true);
      _renderer.EnableWireframe(false);
      _renderer.SetCullMode(CullMode.None);
    }

    public void Render(double time)
    {
      _scene.Render(_renderer);
    }

    public void Update(double time)
    {
      
    }

    public void UpdateSize(double width, double height)
    {
      var perspectiveCamera = _scene.CurrentCamera as PerspectiveCamera;
      if (perspectiveCamera != null)
        perspectiveCamera.AspectRatio = (float)(width / height);
    }

    public void MouseWheel(int delta, int x, int y)
    {

    }

    public void MouseDown(ControllerMouseButton button, int x, int y)
    {
 
    }

    public void MouseUp(ControllerMouseButton button, int x, int y)
    {
    
    }

    public void MouseMove(int x, int y, int deltaX, int deltaY)
    {
    
    }

    public void KeyDown(int key)
    {
      
    }

  }
}