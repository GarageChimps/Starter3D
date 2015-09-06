using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenTK;
using Starter3D.API.controller;
using Starter3D.API.geometry.primitives;
using Starter3D.API.math;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.Plugin.SceneGraph
{
  public class SceneGraphController : ViewModelBase, IController
  {
    private const string ScenePath = @"scenes/interactivescenegraph.xml";
    private const string ResourcePath = @"resources/scenegraphresources.xml";

    private readonly IRenderer _renderer;
    private readonly ISceneReader _sceneReader;
    private readonly IResourceManager _resourceManager;

    private readonly IScene _scene;

    private readonly IEnumerable<InteractiveShapeNode> _interactiveShapes;
    private InteractiveShapeNode _pickableShape;
    private InteractiveShapeNode _selectedShape;
    private InteractiveShapeViewModel _selectedShapeVM;

    private readonly SceneGraphView _centralView;

    private bool _isDragging;
    private bool _isOrbiting;
    private bool _isAnimating;

    private int _width;
    private int _height;

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
      get { return "Scene Graph"; }
    }

    public InteractiveShapeViewModel SelectedShape
    {
      get { return _selectedShapeVM; }
      set
      {
        if (_selectedShapeVM != value)
        {
          _selectedShapeVM = value;
          OnPropertyChanged(() => SelectedShape);
        }
      }
    }


    public SceneGraphController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
    {
      if (renderer == null) throw new ArgumentNullException("renderer");
      if (sceneReader == null) throw new ArgumentNullException("sceneReader");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;

      _resourceManager.Load(ResourcePath);
      _scene = _sceneReader.Read(ScenePath);

      _interactiveShapes = _scene.RootNode.GetNodes<InteractiveShapeNode>();
      _centralView = new SceneGraphView(this);
    }

    public void Load()
    {
      InitRenderer();

      _resourceManager.Configure(_renderer);
      _scene.Configure(_renderer);

    }

    private void InitRenderer()
    {
      _renderer.SetBackgroundColor(0.9f, 0.9f, 1.0f);
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
      if (_isAnimating)
      {
        var s1 = _scene.Shapes.First(s => s.Shape.Name == "sphere");
        var s2 = _scene.Shapes.First(s => s.Shape.Name == "sphere2");
        var s3 = _scene.Shapes.First(s => s.Shape.Name == "sphere3");
        s1.Rotation *= Quaternion.FromAxisAngle(new Vector3(0, 0, 1), 0.1f*(float) time);
        s2.Rotation *= Quaternion.FromAxisAngle(new Vector3(0, 0, 1), 0.2f*(float) time);
        s3.Rotation *= Quaternion.FromAxisAngle(new Vector3(0, 0, 1), 0.4f*(float) time);
      }
    }

    public void UpdateSize(double width, double height)
    {
      _width = (int)width;
      _height = (int)height;
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

      if (_pickableShape != null)
      {
        if (_selectedShape != null)
          _selectedShape.Select(false);
        _pickableShape.Select(true);
        _selectedShape = _pickableShape;
        SelectedShape = new InteractiveShapeViewModel(_selectedShape);
      }
      else if (_selectedShape != null)
      {
        _selectedShape.Select(false);
        SelectedShape = null;
      }
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

      Pick(x, y);
    }

    private void Pick(int x, int y)
    {
      var adjustedX = (2.0f*((float) x/_width) - 1.0f);
      var adjustedY = (2.0f*((float) (_height - y)/_height) - 1.0f);
      var cameraPoint = _scene.CurrentCamera.Position;
      var farPoint = _scene.CurrentCamera.Unproject(new Vector3(adjustedX, adjustedY, -1));
      var direction = (farPoint - cameraPoint).Normalized();
      
      var ray = new Ray(cameraPoint, direction);
      _pickableShape = null;
      foreach (var shape in _interactiveShapes)
      {
        var intersects = shape.TestIntersection(ray);
        if (intersects)
        {
          _pickableShape = shape;
        }
        shape.Highlight(false);
      }
      if (_pickableShape != null)
        _pickableShape.Highlight(true);
    }

    public void KeyDown(int key)
    {
      var s1 = _scene.Shapes.First(s => s.Shape.Name == "sphere");
      var s2 = _scene.Shapes.First(s => s.Shape.Name == "sphere2");
      var s3 = _scene.Shapes.First(s => s.Shape.Name == "sphere3");
      if (key == 'w')
        s1.Rotation *= Quaternion.FromAxisAngle(new Vector3(0, 0, 1), 0.1f);
      else if (key == 's')
        s1.Rotation *= Quaternion.FromAxisAngle(new Vector3(0, 0, 1), -0.1f);
      else if (key == 'e')
        s2.Rotation *= Quaternion.FromAxisAngle(new Vector3(0, 0, 1), 0.1f);
      else if (key == 'd')
        s2.Rotation *= Quaternion.FromAxisAngle(new Vector3(0, 0, 1), -0.1f);
      else if (key == 'r')
        s3.Rotation *= Quaternion.FromAxisAngle(new Vector3(0, 0, 1), 0.1f);
      else if (key == 'f')
        s3.Rotation *= Quaternion.FromAxisAngle(new Vector3(0, 0, 1), -0.1f);
      else if (key == ' ')
        _isAnimating = !_isAnimating;
    }



  }
}