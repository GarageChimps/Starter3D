﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using OpenTK;
using Starter3D.API.controller;
using Starter3D.API.geometry;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;
using Starter3D.Plugin.SceneGraph;

namespace Starter3D.Plugin.Physics
{
  public class PhysicsController : ViewModelBase, IController
  {
    private const string ScenePath = @"scenes/physicsscene.xml";
    private const string ResourcePath = @"resources/physics.xml";

    private readonly IRenderer _renderer;
    private readonly ISceneReader _sceneReader;
    private readonly IResourceManager _resourceManager;

    private readonly IScene _scene;

    private readonly PhysicsView _centralView;

    private readonly List<Asteroid> _asteroids = new List<Asteroid>();

    private ShapeNode _asteroidsNode;
    private MeshCollection _asteroidMeshCollection;

    private bool _isDragging;
    private bool _isOrbiting;

    private int _width;
    private int _height;

    private int _frameCount = 0;
    private float _fps;

    private int _numberOfAsteroids = 1000;
    private float _minRadius = 90;
    private float _maxRadius = 110;
    private float _minSpeed = 0.3f;
    private float _maxSpeed = 0.7f;
    private float _minSize = 0.5f;
    private float _maxSize = 3f;

    private float t = 0.0f;

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
      get { return "Physics"; }
    }

    public float FPS
    {
      get { return _fps; }
      set
      {
        if (_fps != value)
        {
          _fps = value;
          OnPropertyChanged(() => FPS);
        }
      }
    }


    public PhysicsController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
    {
      if (renderer == null) throw new ArgumentNullException("renderer");
      if (sceneReader == null) throw new ArgumentNullException("sceneReader");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;

      _resourceManager.Load(ResourcePath);
      _scene = _sceneReader.Read(ScenePath);

      _centralView = new PhysicsView(this);
    }

    public void Load()
    {
      var random = new Random();
      for (int i = 0; i < _numberOfAsteroids; i++)
      {
        var radius = _minRadius + (float) (random.NextDouble()*_maxRadius);
        var speed = _minSpeed + (float) (random.NextDouble()*_maxSpeed);
        var size = _minSize + (float)(random.NextDouble() * _maxSize);
        _asteroids.Add(new Asteroid(radius, speed, new Vector3(), size));
      }
      _asteroidsNode = _scene.Shapes.First(s => s.Shape.Name == "asteroid");
      var mesh = _asteroidsNode.Shape as IMesh;
      mesh.Material = _resourceManager.GetMaterial("whiteInstancing");
      _asteroidMeshCollection = new MeshCollection(mesh.Name, mesh);
      _asteroidMeshCollection.Material = _resourceManager.GetMaterial("whiteInstancing");
      _asteroidsNode.Shape = _asteroidMeshCollection;

      InitRenderer();

      _resourceManager.Configure(_renderer);
      _scene.Configure(_renderer);

    }
   
    private void InitRenderer()
    {
      _renderer.SetBackgroundColor(0.0f, 0.0f, 0.0f);
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
      _frameCount++;
      if (_frameCount % 10 == 0)
        FPS = 1.0f / (float)time;
      t += (float)time;
      UpdateAsteroids(t);

    }

    private void UpdateAsteroids(float time)
    {
      _asteroidMeshCollection.Clear();
      foreach (var asteroid in _asteroids)
      {
        _asteroidMeshCollection.AddInstance(asteroid.GetTransformantion(time));
      }
      _asteroidMeshCollection.Configure(_renderer);
    }

    public void UpdateSize(double width, double height)
    {
      _width = (int)width;
      _height = (int)height;
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

    private void Pick(int x, int y)
    {

    }

    public void KeyDown(int key)
    {

    }



  }
}