using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using OpenTK.Graphics;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;

namespace Starter3D.Plugin.MaterialEditor
{
  public class MaterialEditorController : IController
  {
    private const string ScenePath = @"scenes/testCamera.xml";
    private const string ResourcePath = @"resources/resources.xml";

    private readonly IRenderer _renderer;
    private readonly ISceneReader _sceneReader;
    private readonly IResourceManager _resourceManager;

    private readonly ISceneNode _sceneGraph;
    private readonly IEnumerable<ShapeNode> _objects;
    private readonly IEnumerable<LightNode> _lights;
    private readonly IEnumerable<CameraNode> _cameras;
    private readonly IEnumerable<IMaterial> _materials;
    
    private PerspectiveCamera _camera;
    private ShapeNode _shape;
    private PointLight _light;
    private AmbientLight _ambientLight;
    private int _currentShape = 0;
    private int _currentMaterial = 0;

    private readonly MaterialEditorView _view;

    private bool _isDragging;
    private bool _isOrbiting;


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

    public object View
    {
      get { return _view; }
    }

    public bool HasUserInterface
    {
      get { return true; }
    }

    public MaterialEditorController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
    {
      if (renderer == null) throw new ArgumentNullException("renderer");
      if (sceneReader == null) throw new ArgumentNullException("sceneReader");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;

      _resourceManager.Load(ResourcePath);
      _materials = _resourceManager.GetMaterials();
      _sceneGraph = _sceneReader.Read(ScenePath);
      _sceneGraph.GetNodes<ISceneNode>().ToList();
      _objects = _sceneGraph.GetNodes<ShapeNode>().ToList();
      _lights = _sceneGraph.GetNodes<LightNode>().ToList();
      _cameras = _sceneGraph.GetNodes<CameraNode>().ToList();

      _view = new MaterialEditorView();
      _view.DataContext = this;
    }
    
    public void Load()
    {
      InitRenderer();

      _light = _sceneGraph.GetNodes<PointLight>().First();
      _camera = _sceneGraph.GetNodes<PerspectiveCamera>().First();
      foreach (var material in _materials)
      {
        material.Configure(_renderer);
      }
      var pointLights = _sceneGraph.GetNodes<PointLight>().ToList();
      for (int i = 0; i < pointLights.Count(); i++)
      {
        pointLights.ElementAt(i).Index = i;
      }
      var directionalLights = _sceneGraph.GetNodes<DirectionalLight>().ToList();
      for (int i = 0; i < directionalLights.Count(); i++)
      {
        directionalLights.ElementAt(i).Index = i;
      }
      _ambientLight = _sceneGraph.GetNodes<AmbientLight>().First();
      foreach (var obj in _objects)
      {
        obj.Configure(_renderer);
      }
      foreach (var camera in _cameras)
      {
        camera.Configure(_renderer);
      }
      foreach (var light in _lights)
      {
        light.Configure(_renderer);
      }
      _renderer.SetNumberParameter("activeNumberOfPointLights", pointLights.Count());
      _renderer.SetNumberParameter("activeNumberOfDirectionalLights", directionalLights.Count());

      NextMaterial();
      NextShape();
    }

    private void InitRenderer()
    {
      _renderer.SetBackgroundColor(Color4.White);
      _renderer.EnableZBuffer(true);
    }

    public void Render(double time)
    {
      _camera.Render(_renderer);
      foreach (var light in _lights)
      {
        light.Render(_renderer);
      }
      _ambientLight.Render(_renderer);
      _shape.Render(_renderer);
    }

    public void Update(double time)
    {
      
    }

    public void UpdateSize(double width, double height)
    {
      _camera.AspectRatio = (float)(width / height);
    }

    public void MouseWheel(int delta, int x, int y)
    {
      _camera.Zoom(delta);
      _light.Position = _camera.Position;
    }

    public void MouseDown(ControllerMouseButton button, int x, int y)
    {
      if (button == ControllerMouseButton.Right)
        _isDragging = true;
      else if (button == ControllerMouseButton.Left)
        _isOrbiting = true;
    }

    public void MouseUp(ControllerMouseButton button, int x, int y)
    {
      if (button == ControllerMouseButton.Right)
        _isDragging = false;
      else if (button == ControllerMouseButton.Left)
        _isOrbiting = false;
    }

    public void MouseMove(int x, int y, int deltaX, int deltaY)
    {
      if (_isDragging)
        _camera.Drag(deltaX, deltaY);
      else if (_isOrbiting)
        _camera.Orbit(deltaX, deltaY);
      _light.Position = _camera.Position;
    }

    public void KeyDown(int key)
    {
      if (key == 'm') 
      {
        NextMaterial();
      }
      else if (key == 's') 
      {
        NextShape();
      }
    }

    private void NextShape()
    {
      _shape = _objects.ElementAt(_currentShape);
      _currentShape = (_currentShape + 1)%_objects.Count();
    }

    private void NextMaterial()
    {
      var material = _materials.ElementAt(_currentMaterial);
      _currentMaterial = (_currentMaterial + 1)%_materials.Count();
      foreach (var shapeNode in _objects)
      {
        shapeNode.Shape.Material = material;
      }
    }
  }
}