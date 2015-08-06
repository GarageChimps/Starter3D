using System.Linq;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;

namespace Starter3D.Plugin.MaterialEditor
{
  public class MaterialEditorController : BaseController
  {
    private const string Scene = @"scenes/testCamera.xml";
    private const string Resources = @"resources/resources.xml";

    private readonly CameraNode _camera;
    private ShapeNode _shape;
    private readonly PointLight _light;
    private AmbientLight _ambientLight;
    private int _currentShape = 0;
    private int _currentMaterial = 0;

    private bool _isDragging;
    private bool _isOrbiting;

    public MaterialEditorController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
      : base(renderer, sceneReader, resourceManager)
    {
      Init(Scene, Resources);
      _light = _sceneGraph.GetNodes<PointLight>().First();
      _camera = _cameras.First();
    }

    public override void Load()
    {
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