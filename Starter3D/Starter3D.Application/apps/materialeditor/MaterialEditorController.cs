using System.Collections.Generic;
using System.Linq;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.Application.apps.materialeditor
{
  public class MaterialEditorController : BaseController
  {
    private readonly ISceneNode _sceneGraph;
    private readonly List<ISceneNode> _sceneElements = new List<ISceneNode>();
    private readonly IEnumerable<ShapeNode> _objects;
    private readonly IEnumerable<LightNode> _lights;
    private readonly IEnumerable<CameraNode> _cameras;
    private CameraNode _camera;
    private ShapeNode _shape;
    private PointLight _light;
    private AmbientLight _ambientLight;
    private int _currentShape = 0;

    private readonly IEnumerable<IMaterial> _materials;
    private int _currentMaterial = 0;

    private bool _isDragging;
    private bool _isOrbiting;

    public MaterialEditorController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager, IConfiguration configuration)
      : base(renderer, sceneReader, resourceManager, configuration)
    {
      _resourceManager.Load(configuration.ResourcesPath);
      _materials = _resourceManager.GetMaterials();
      _sceneGraph = _sceneReader.Read(configuration.ScenePath);
      _objects = _sceneGraph.GetNodes<ShapeNode>().ToList();
      _lights = _sceneGraph.GetNodes<LightNode>().ToList();
      _light = _sceneGraph.GetNodes<PointLight>().First();
      _cameras = _sceneGraph.GetNodes<CameraNode>().ToList();
      _camera = _cameras.First();
      _sceneElements.AddRange(_objects);
      _sceneElements.AddRange(_lights);
      _sceneElements.Add(_camera);
      
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
      if (key == 51) //Space
      {
        NextMaterial();
      }
      else if (key == 49) //Enter
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