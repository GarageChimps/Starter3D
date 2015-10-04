using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using OpenTK;
using Starter3D.API.controller;
using Starter3D.API.geometry;
using Starter3D.API.physics;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

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

    private readonly List<OrbitalBody> _asteroids = new List<OrbitalBody>();

    private ShapeNode _asteroidsNode;
    private MeshCollection _asteroidMeshCollection;

    private IPhysicsEngine _engine;

    private bool _isDragging;
    private bool _isOrbiting;

    private int _width;
    private int _height;

    private int _frameCount = 0;
    private float _fps;

    private int _numberOfAsteroids = 3000;
    private float _minRadius = 100;
    private float _maxRadius = 150;
    private float _minSpeed = 10f;
    private float _maxSpeed = 15f;
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

      //_centralView = new PhysicsView(this);

      _engine = new PhysicsEngine(new RungeKuttaSolver());
    }

    public void Load()
    {
      var earth = new OrbitalBody(new Vector3(), new Vector3(), 50000, 10);
      var random = new Random();
      for (int i = 0; i < _numberOfAsteroids; i++)
      {
        var x = (float)(2.0f * (random.NextDouble() - 0.5));
        var y = (float)(2.0f * (random.NextDouble() - 0.5));
        var position = _minRadius * new Vector3(x,0,y) + (_maxRadius - _minRadius) * new Vector3(x,0,y);
        var speed = _minSpeed + (float)(random.NextDouble() * _maxSpeed);
        var speedDirection = Vector3.Cross(position.Normalized(), new Vector3(0, 1, 0)).Normalized();
        var size = _minSize + (float)(random.NextDouble() * _maxSize);
        var asteroid = new OrbitalBody(position, speed * speedDirection, 1, size);
        _asteroids.Add(asteroid);
        _engine.AddObject(asteroid);
      }
      _engine.AddForce(new Gravity(earth));

      _asteroidsNode = _scene.Shapes.First(s => s.Shape.Name == "asteroid");
      var mesh = _asteroidsNode.Shape as IMesh;
      mesh.Material = _resourceManager.GetMaterial("whiteInstancing");
      _asteroidMeshCollection = new MeshCollection(mesh.Name, mesh);
      _asteroidMeshCollection.Material = _resourceManager.GetMaterial("whiteInstancing");
      _asteroidsNode.Shape = _asteroidMeshCollection;
      UpdateAsteroids(0, true);

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
      UpdateAsteroids(t);
      _scene.Render(_renderer);
    }

    public void Update(double time)
    {
      _frameCount++;
      if (_frameCount % 10 == 0)
        FPS = 1.0f / (float)time;
      t += (float)time;
      _engine.Update((float)time);
    }

    private void UpdateAsteroids(float time, bool firstTime=false)
    {
      _asteroidMeshCollection.Clear();
      foreach (var asteroid in _asteroids)
      {
        _asteroidMeshCollection.AddInstance(asteroid.GetTransformantion(time));
      }
      if (firstTime)
        _asteroidMeshCollection.Configure(_renderer);
      else
        _asteroidMeshCollection.Update(_renderer);
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