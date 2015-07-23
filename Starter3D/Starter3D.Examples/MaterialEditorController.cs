using System.Collections.Generic;
using System.Linq;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.Examples
{
  public class MaterialEditorController : BaseController
  {
    private readonly ISceneNode _sceneGraph;
    private readonly IEnumerable<ShapeNode> _objects;
    private readonly IEnumerable<LightNode> _lights;
    private readonly IEnumerable<CameraNode> _cameras;
    private readonly CameraNode _camera;
    private readonly List<ISceneNode> _sceneElements = new List<ISceneNode>();

    public MaterialEditorController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager, IConfiguration configuration)
      : base(renderer, sceneReader, resourceManager, configuration)
    {
      _resourceManager.Load(configuration.ResourcesPath);
      _sceneGraph = _sceneReader.Read(configuration.ScenePath);
      _objects = _sceneGraph.GetNodes<ShapeNode>();
      _lights = _sceneGraph.GetNodes<LightNode>();
      _cameras = _sceneGraph.GetNodes<CameraNode>();
      _camera = _cameras.First();
      _sceneElements.AddRange(_objects);
      _sceneElements.AddRange(_lights);
      _sceneElements.Add(_camera);
    }

    public override void Load()
    {
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
    }

    public override void Render(double time)
    {
      foreach (var camera in _cameras)
      {
        camera.Render(_renderer);
      }
      foreach (var light in _lights)
      {
        light.Render(_renderer);
      }
      foreach (var obj in _objects)
      {
        obj.Render(_renderer);
      }
    }
  }
}