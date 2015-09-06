using System;
using System.Linq;
using OpenTK;
using Starter3D.API.controller;
using Starter3D.API.geometry;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.Plugin.CurveEditor
{
  public class CurveEditorController : IController
  {
    private const string ScenePath = @"scenes/curvescene.xml";
    private const string ResourcePath = @"resources/curveresources.xml";

    private readonly IRenderer _renderer;
    private readonly ISceneReader _sceneReader;
    private readonly IResourceManager _resourceManager;

    private CurveEditorView _view;

    private ICurve _curve;
    private Spline _spline;
    private float _step = 0.1f;

    private double _width;
    private double _height;


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
      get { return _view; }
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
      get { return "Curve Editor"; }
    }

    public CurveEditorController(IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager)
    {
      if (renderer == null) throw new ArgumentNullException("renderer");
      if (sceneReader == null) throw new ArgumentNullException("sceneReader");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;

      _resourceManager.Load(ResourcePath);
    }

    public void Load()
    {
      InitRenderer();

      _resourceManager.Configure(_renderer);
      _curve = new Curve("curve1");
      _curve.Material = _resourceManager.GetMaterials().First();
      _curve.Configure(_renderer);

      _spline = new CatmullRom();

    }

    private void InitRenderer()
    {
      _renderer.SetBackgroundColor(0.0f, 0.0f, 0.0f);
      _renderer.EnableZBuffer(false);
      _renderer.EnableWireframe(false);
      _renderer.SetCullMode(CullMode.None);
    }

    public void Render(double time)
    {
      _curve.Render(_renderer, Matrix4.Identity);
    }

    public void Update(double deltaTime)
    {

    }

    public void MouseDown(ControllerMouseButton button, int x, int y)
    {
      float adjustedX = (2.0f*(float) x/(float) _width) - 1;
      float adjustedY = (2.0f*(float) (_height - y)/(float) _height) - 1;
      _spline.AddPoint(new Vector3(adjustedX, adjustedY, 0));
      if (_spline.Points.Count >= 4)
      {
        _curve.Clear();
        var points = _spline.Interpolate(_step);
        foreach (var point in points)
        {
          _curve.AddPoint(point);
        }
        _curve.Configure(_renderer);
      }
    }

    public void MouseUp(ControllerMouseButton button, int x, int y)
    {

    }

    public void MouseWheel(int delta, int x, int y)
    {

    }

    public void MouseMove(int x, int y, int deltaX, int deltaY)
    {

    }

    public void KeyDown(int key)
    {

    }

    public void UpdateSize(double width, double height)
    {
      _width = width;
      _height = height;
    }
  }
}
