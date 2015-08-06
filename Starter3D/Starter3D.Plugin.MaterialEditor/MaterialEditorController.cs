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
  public class MaterialEditorController : BaseController
  {
    private PerspectiveCamera _camera;
    private ShapeNode _shape;
    private PointLight _light;
    private AmbientLight _ambientLight;
    private int _currentShape = 0;
    private int _currentMaterial = 0;

    private bool _isDragging;
    private bool _isOrbiting;

    protected override string ScenePath
    {
      get { return @"scenes/testCamera.xml"; }
    }

    protected override string ResourcePath
    {
      get { return @"resources/resources.xml"; }
    }

    public MaterialEditorController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
      : base(renderer, sceneReader, resourceManager)
    {
      
    }
    
    public override void Load()
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

    public override void Render(double time)
    {
      _camera.Render(_renderer);
      foreach (var light in _lights)
      {
        light.Render(_renderer);
      }
      _ambientLight.Render(_renderer);
      _shape.Render(_renderer);
    }

    public override void UpdateSize(double width, double height)
    {
      _camera.AspectRatio = (float)(width / height);
    }

    public override void MouseWheel(int delta, int x, int y)
    {
      _camera.Zoom(delta);
      _light.Position = _camera.Position;
    }

    public override void MouseDown(ControllerMouseButton button, int x, int y)
    {
      if (button == ControllerMouseButton.Right)
        _isDragging = true;
      else if (button == ControllerMouseButton.Left)
        _isOrbiting = true;
    }

    public override void MouseUp(ControllerMouseButton button, int x, int y)
    {
      if (button == ControllerMouseButton.Right)
        _isDragging = false;
      else if (button == ControllerMouseButton.Left)
        _isOrbiting = false;
    }

    public override void MouseMove(int x, int y, int deltaX, int deltaY)
    {
      if (_isDragging)
        _camera.Drag(deltaX, deltaY);
      else if (_isOrbiting)
        _camera.Orbit(deltaX, deltaY);
      _light.Position = _camera.Position;
    }

    public override void KeyDown(int key)
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