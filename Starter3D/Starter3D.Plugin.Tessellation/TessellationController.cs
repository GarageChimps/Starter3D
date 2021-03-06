﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Starter3D.API.controller;
using Starter3D.API.geometry.primitives;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.Plugin.Tessellation
{
  public class TesselationController : ViewModelBase, IController 
  {
    private const string ScenePath = @"scenes/tessellationScene.xml";
    private const string ResourcePath = @"resources/tessellationResources.xml";

    private readonly IRenderer _renderer;
    private readonly ISceneReader _sceneReader;
    private readonly IResourceManager _resourceManager;

    private readonly IScene _scene;
    private TesselatedMesh _tesselatedMesh;
    
    private readonly TessellationView _centralView;
    private readonly ObservableCollection<MaterialViewModel> _materials = new ObservableCollection<MaterialViewModel>();
    private MaterialViewModel _currentMaterial;

    private bool _isDragging;
    private bool _isOrbiting;

    private int _numU;
    private int _numV;

    private bool _showWireframe;

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
      get { return "Simple Material Editor"; }
    }

    public int NumU
    {
      get { return _numU; }
      set
      {
        if (_numU != value)
        {
          _numU = value;
          OnPropertyChanged(() => NumU);
          Tessellate();
        }
      }
    }

   
    public int NumV
    {
      get { return _numV; }
      set
      {
        if (_numV != value)
        {
          _numV = value;
          OnPropertyChanged(() => NumV);
          Tessellate();
        }
      }
    }

    public bool ShowWireframe
    {
      get { return _showWireframe; }
      set
      {
        if (_showWireframe != value)
        {
          _showWireframe = value;
          OnPropertyChanged(() => ShowWireframe);
          _renderer.EnableWireframe(_showWireframe);
        }
      }
    }

    public ObservableCollection<MaterialViewModel> Materials
    {
      get { return _materials; }
    }

    public MaterialViewModel CurrentMaterial
    {
      get { return _currentMaterial; }
      set
      {
        if (_currentMaterial != value)
        {
          _currentMaterial = value;
          _tesselatedMesh.Material = _currentMaterial.Material;
          OnPropertyChanged(() => CurrentMaterial);
        }
      }
    }

    public TesselationController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
    {
      if (renderer == null) throw new ArgumentNullException("renderer");
      if (sceneReader == null) throw new ArgumentNullException("sceneReader");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;

      _resourceManager.Load(ResourcePath);
      _scene = _sceneReader.Read(ScenePath);
      _tesselatedMesh = _scene.Shapes.First().Shape as TesselatedMesh;
      _numU = _tesselatedMesh.NumU;
      _numV = _tesselatedMesh.NumV;
      //_scene.Shapes.First().Shape = new Cube();
      //_scene.Shapes.First().Shape.Material = _resourceManager.GetMaterials().First();
      
      _centralView = new TessellationView();
      _centralView.DataContext = this;
    }

    private void Tessellate()
    {
      _tesselatedMesh.Material.SetParameter("numU", NumU);
      _tesselatedMesh.Material.SetParameter("numV", NumV);
      _tesselatedMesh.Tesselate(NumU, NumV);
      _tesselatedMesh.Update(_renderer);
    }

    
    public void Load()
    {
      InitRenderer();

      _resourceManager.Configure(_renderer);
      _scene.Configure(_renderer);

      foreach (var material in _resourceManager.GetMaterials())
      {
        _materials.Add(new MaterialViewModel(material));
      }
      CurrentMaterial = _materials.First();

    }

    private void InitRenderer()
    {
      _renderer.SetBackgroundColor(0.9f,0.9f,1.0f);
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
      
    }

    public void UpdateSize(double width, double height)
    {
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

    }

    public void KeyDown(int key)
    {
      
    }

  }
}