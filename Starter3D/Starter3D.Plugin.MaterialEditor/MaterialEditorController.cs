using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenTK.Graphics;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;

namespace Starter3D.Plugin.MaterialEditor
{
  public class MaterialEditorController : ViewModelBase, IController
  {
    private const string ScenePath = @"scenes/scene.xml";
    private const string ResourcePath = @"resources/resources.xml";

    private readonly IRenderer _renderer;
    private readonly ISceneReader _sceneReader;
    private readonly IResourceManager _resourceManager;

    private readonly IScene _scene;
    private readonly IScene _originalScene;
    private readonly IEnumerable<ShapeNode> _shapes;


    private readonly MaterialEditorView _centralView;
    private readonly ObservableCollection<MaterialViewModel> _materials = new ObservableCollection<MaterialViewModel>();
    private MaterialViewModel _currentMaterial;
    private readonly ObservableCollection<ShapeViewModel> _shapeViewModels = new ObservableCollection<ShapeViewModel>();
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

    public ObservableCollection<ShapeViewModel> ShapeViewModels
    {
      get { return _shapeViewModels; }
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
      
      //AE August 2015: The scene definition from file contains multiple shapes, we only want to render one, so we create a new scene with only the first shape
      _originalScene = _sceneReader.Read(ScenePath);
      _scene = new Scene(_originalScene.Cameras.ToList(), new List<ShapeNode>{_originalScene.Shapes.First()}, _originalScene.Lights.ToList() );
      _shapes = _originalScene.Shapes;

      _centralView = new MaterialEditorView();
      _centralView.DataContext = this;
    }
    
    public void Load()
    {
      InitRenderer();

      _resourceManager.Configure(_renderer);
      _originalScene.Configure(_renderer);

      InitView();
    }

    private void InitView()
    {
      foreach (var material in _resourceManager.GetMaterials())
      {
        _materials.Add(new MaterialViewModel(material));
      }
      CurrentMaterial = _materials.First();
      foreach (var obj in _shapes)
      {
        _shapeViewModels.Add(new ShapeViewModel(obj));
      }
      CurrentShape = ShapeViewModels.First();
    }

    private void InitRenderer()
    {
      _renderer.SetBackgroundColor(new Color4(0.9f,0.9f,1.0f,1));
      _renderer.EnableZBuffer(true);
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
      _scene.CurrentCamera.Zoom(delta);
      var pointLight = _scene.Lights.First() as PointLight;
      if (pointLight != null)
        pointLight.Position = _scene.CurrentCamera.Position;
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
        _scene.CurrentCamera.Drag(deltaX, deltaY);
      else if (_isOrbiting)
        _scene.CurrentCamera.Orbit(deltaX, deltaY);
      var pointLight = _scene.Lights.First() as PointLight;
      if (pointLight != null)
        pointLight.Position = _scene.CurrentCamera.Position;
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
      if (_currentShape != null)
      {
        _scene.ClearShapes();
        _scene.AddShape(_currentShape.Shape);
      }
      if (_currentShape != null && _currentMaterial != null)
        _currentShape.Shape.Shape.Material = _currentMaterial.Material;
    }

  }
}