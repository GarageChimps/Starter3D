using System.Linq;
using OpenTK;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;

namespace Starter3D.Plugin.PixelShader
{
  public class PixelShaderController : BaseController
  {
    private const string Scene = @"scenes/pixelShaderScene.xml";
    private const string Resources = @"resources/pixelShaderResources.xml";

    private readonly ShapeNode _shape;
    private int _currentMaterial = 0;
    private Vector3 _currentMousePosition;

    private double _accumulatedTime = 0;
    private double _width = 1;
    private double _height = 1;

    public PixelShaderController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
      : base(renderer, sceneReader, resourceManager)
    {
      Init(Scene, Resources);
      _shape = _objects.First();
    }

    public override void UpdateSize(double width, double height)
    {
      _width = width;
      _height = height;
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
      _currentMousePosition = new Vector3(x/(float)_width, 1.0f - y/(float)_height, 0);
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