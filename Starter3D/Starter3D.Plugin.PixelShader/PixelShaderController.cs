using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenTK;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;

namespace Starter3D.Plugin.PixelShader
{
  public class PixelShaderController : ViewModelBase, IController
  {
    private const string ScenePath = @"scenes/pixelShaderScene.xml";
    private const string ResourcePath = @"resources/pixelShaderResources.xml";

    private readonly IRenderer _renderer;
    private readonly ISceneReader _sceneReader;
    private readonly IResourceManager _resourceManager;

    private readonly ISceneNode _sceneGraph;
    private readonly IEnumerable<ShapeNode> _objects;
    private ShapeNode _shape;
    private IMaterial _currentMaterial;
    private Vector3 _currentMousePosition;

    private double _accumulatedTime = 0;
    private double _width = 1;
    private double _height = 1;

    private readonly PixelShaderView _centralView;
    private readonly ObservableCollection<ShaderViewModel> _shaders = new ObservableCollection<ShaderViewModel>();
    private ShaderViewModel _currentShader;


    public int Width
    {
      get { return 512; }
    }

    public int Height
    {
      get { return 512; }
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
      get { return "Pixel Shader"; }
    }

    public ObservableCollection<ShaderViewModel> Shaders
    {
      get { return _shaders; }
    }

    public ShaderViewModel CurrentShader
    {
      get { return _currentShader; }
      set
      {
        if (_currentShader != value)
        {
          _currentShader = value;
          OnCurrentShaderChanged();
          OnPropertyChanged(() => CurrentShader);
        }
      }
    }

    public PixelShaderController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
    {
      if (renderer == null) throw new ArgumentNullException("renderer");
      if (sceneReader == null) throw new ArgumentNullException("sceneReader");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;

      _resourceManager.Load(ResourcePath);
      _resourceManager.GetMaterials();
      _sceneGraph = _sceneReader.Read(ScenePath);
      _sceneGraph.GetNodes<ISceneNode>().ToList();
      _objects = _sceneGraph.GetNodes<ShapeNode>().ToList();
      _sceneGraph.GetNodes<LightNode>().ToList();
      _sceneGraph.GetNodes<CameraNode>().ToList();

      _centralView = new PixelShaderView();
      _centralView.DataContext = this;
    }

    public void UpdateSize(double width, double height)
    {
      _width = width;
      _height = height;
    }

    public void Load()
    {
      var shaders = _resourceManager.GetShaders().ToList();
      foreach (var shader in shaders)
      {
        shader.Configure(_renderer);
      }
      foreach (var shader in shaders)
      {
        _shaders.Add(new ShaderViewModel(shader));
      }
      CurrentShader = _shaders.First();

      _currentMaterial = new Material(shaders.First());
      _currentMaterial.Configure(_renderer);

      _shape = _objects.First();
      _shape.Shape.Material = _currentMaterial;
      _shape.Configure(_renderer);
    }


    public void Render(double time)
    {
      _accumulatedTime += time;
      _renderer.SetNumericParameter("time", (float)_accumulatedTime);
      _renderer.SetVectorParameter("mouse", _currentMousePosition);
      _shape.Render(_renderer);
    }

    public void Update(double time)
    {

    }

    public void MouseDown(ControllerMouseButton button, int x, int y)
    {

    }

    public void MouseUp(ControllerMouseButton button, int x, int y)
    {

    }

    public void MouseWheel(int delta, int x, int y)
    {

    }

    public void MouseMove(int x, int y, int deltaX, int deltaY)
    {
      _currentMousePosition = new Vector3(x / (float)_width, 1.0f - y / (float)_height, 0);
    }

    public void KeyDown(int key)
    {

    }

    private void SetCurrentShader(IShader shader)
    {
      if (_currentMaterial != null)
      {
        _currentMaterial.Shader = shader;
      }
    }

    private void OnCurrentShaderChanged()
    {
      SetCurrentShader(_currentShader.Shader);
    }

  }
}