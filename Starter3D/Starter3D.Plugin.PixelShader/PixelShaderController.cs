using System.Linq;
using OpenTK;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.Plugin.PixelShader
{
  public class PixelShaderController : BaseController
  {
    private readonly ShapeNode _shape;
    private int _currentMaterial = 0;
    private Vector3 _currentMousePosition;

    private double _accumulatedTime = 0;

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
      _accumulatedTime += time;
      _renderer.SetNumberParameter("time", (float)_accumulatedTime);
      _renderer.SetVectorParameter("mouse", _currentMousePosition);
      _shape.Render(_renderer);
    }

    public override void MouseMove(int x, int y, int deltaX, int deltaY)
    {
      //AE August 2015: Hardcoded sizes for now. Controller should have acces to image size apparently...
      _currentMousePosition = new Vector3(x/512.0f, 1.0f - y/512.0f, 0);
    }

    public override void KeyDown(int key)
    {
      NextMaterial();
    }

    private void NextMaterial()
    {
      var material = _materials.ElementAt(_currentMaterial);
      _currentMaterial = (_currentMaterial + 1) % _materials.Count();
      _shape.Shape.Material = material;
    }
  }
}