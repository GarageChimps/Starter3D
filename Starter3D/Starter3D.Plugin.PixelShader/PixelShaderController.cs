using System.Collections.Generic;
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

    private ShapeNode _shape;
    private IMaterial _currentMaterial;
    private Vector3 _currentMousePosition;

    private double _accumulatedTime = 0;
    private double _width = 1;
    private double _height = 1;

    protected override string ScenePath
    {
      get { return @"scenes/pixelShaderScene.xml"; }
    }

    protected override string ResourcePath
    {
      get { return @"resources/pixelShaderResources.xml"; }
    }

    public PixelShaderController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
      : base(renderer, sceneReader, resourceManager)
    {
      
    }

    public override void UpdateSize(double width, double height)
    {
      _width = width;
      _height = height;
    }

  

    public override void Load()
    {
      var shaders = _resourceManager.GetShaders().ToList();
      foreach (var shader in shaders)
      {
        shader.Configure(_renderer);
      }
      _currentMaterial = new Material(shaders.First());
      _currentMaterial.Configure(_renderer);
      
      _shape = _objects.First();
      _shape.Shape.Material = _currentMaterial;
      _shape.Configure(_renderer);
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
    
    public IEnumerable<IShader> GetShaders()
    {
      return _resourceManager.GetShaders();
    }

    public void SetCurrentShader(IShader shader)
    {
      if (_currentMaterial != null)
      {
        _currentMaterial.Shader = shader;
      }
    }

  }
}