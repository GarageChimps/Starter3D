using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenTK.Graphics;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;

namespace Starter3D.Plugin.MaterialEditor
{
  public class MaterialEditorController : ViewModelBase, IController
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
    
    private PerspectiveCamera _camera;
    private PointLight _light;
    private AmbientLight _ambientLight;

    private readonly MaterialEditorView _centralView;
    private readonly ObservableCollection<MaterialViewModel> _materials = new ObservableCollection<MaterialViewModel>();
    private MaterialViewModel _currentMaterial;
    private readonly ObservableCollection<ShapeViewModel> _shapes = new ObservableCollection<ShapeViewModel>();
    private ShapeViewModel _currentShape;

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
      get { return "Material Editor"; }
    }

    public ObservableCollection<MaterialViewModel> Materials
    {
      get { return _materials; }
    }

    public ObservableCollection<ShapeViewModel> Shapes
    {
      get { return _shapes; }
    }

    public MaterialViewModel CurrentMaterial
    {
      get { return _currentMaterial; }
      set
      {
        if (_currentMaterial != value)
        {
          _currentMaterial = value;
          OnCurrentMaterialChanged();
          OnPropertyChanged(() => CurrentMaterial);
        }
      }
    }
    
    public ShapeViewModel CurrentShape
    {
      get { return _currentShape; }
      set
      {
        if (_currentShape != value)
        {
          _currentShape = value;
          OnCurrentShapeChanged();
          OnPropertyChanged(() => CurrentShape);
        }
      }
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
      _sceneGraph = _sceneReader.Read(ScenePath);
      _sceneGraph.GetNodes<ISceneNode>().ToList();
      _objects = _sceneGraph.GetNodes<ShapeNode>().ToList();
      _lights = _sceneGraph.GetNodes<LightNode>().ToList();
      _cameras = _sceneGraph.GetNodes<CameraNode>().ToList();

      _centralView = new MaterialEditorView();
      _centralView.DataContext = this;
    }
    
    public void Load()
    {
      InitRenderer();

      _light = _sceneGraph.GetNodes<PointLight>().First();
      _camera = _sceneGraph.GetNodes<PerspectiveCamera>().First();
      var materials = _resourceManager.GetMaterials();
      foreach (var material in materials)
      {
        material.Configure(_renderer);
        _materials.Add(new MaterialViewModel(material));
      }
      CurrentMaterial = _materials.First();
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
        _shapes.Add(new ShapeViewModel(obj));
      }
      CurrentShape = Shapes.First();
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

    }

    private void InitRenderer()
    {
      _renderer.SetBackgroundColor(new Color4(0.9f,0.9f,1.0f,1));
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
      _currentShape.Shape.Render(_renderer);
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
     
    }


    private void OnCurrentMaterialChanged()
    {
      if (_currentShape != null && _currentMaterial != null)
        _currentShape.Shape.Shape.Material = _currentMaterial.Material;
    }

    private void OnCurrentShapeChanged()
    {
      if (_currentShape != null && _currentMaterial != null)
        _currentShape.Shape.Shape.Material = _currentMaterial.Material;
    }

  }
}