using System.Linq;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.Application.controllers
{
  public class PixelShaderController : BaseController
  {
    private readonly ShapeNode _shape;
    private int _currentMaterial = 0;

    public PixelShaderController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager, IConfiguration configuration)
      : base(renderer, sceneReader, resourceManager, configuration)
    {
      _shape = _objects.First();
    }

    public override void Load()
    {
      foreach (var material in _materials)
      {
        material.Configure(_renderer);
      }
      _shape.Configure(_renderer);
      NextMaterial();
    }

    public override void Render(double time)
    {
      _shape.Render(_renderer);
    }

    public override void KeyDown(int key)
    {
      if (key == 51) //Space
      {
        NextMaterial();
      }
    }

    private void NextMaterial()
    {
      var material = _materials.ElementAt(_currentMaterial);
      _currentMaterial = (_currentMaterial + 1) % _materials.Count();
      _shape.Shape.Material = material;
    }
  }
}