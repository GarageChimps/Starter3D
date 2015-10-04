using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using OpenTK;
using Starter3D.API.controller;
using Starter3D.API.geometry;
using Starter3D.API.geometry.primitives;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.Plugin.CurveEditor
{
  public class CurveEditorController : ViewModelBase, IController 
  {
    private const string ScenePath = @"scenes/curvescene.xml";
    private const string ResourcePath = @"resources/curveresources.xml";

    private readonly IRenderer _renderer;
    private readonly ISceneReader _sceneReader;
    private readonly IResourceManager _resourceManager;

    private CurveEditorView _view;
    private ObservableCollection<SplineViewModel> _splines = new ObservableCollection<SplineViewModel>();
    private SplineViewModel _selectedSpline;

    private IPoints _points;
    private ICurve _curve;

    private float _step = 0.1f;

    private double _width;
    private double _height;

    private bool _firstPoint = true;
    private bool _firstCurve = true;

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

    public float Step
    {
      get { return _step; }
      set
      {
        if (_step != value)
        {
          _step = value;
          OnPropertyChanged(() => Step);
          UpdateCurve();
        }
      }
    }

    public SplineViewModel SelectedSpline
    {
      get { return _selectedSpline; }
      set
      {
        if (_selectedSpline != value)
        {
          _selectedSpline = value;
          UpdateCurve();
          OnPropertyChanged(() => SelectedSpline);
        }
      }
    }

    public ObservableCollection<SplineViewModel> Splines
    {
      get { return _splines; }
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
      _view = new CurveEditorView(this);
    }

    public void Load()
    {
      InitRenderer();

      _resourceManager.Configure(_renderer);
      _curve = new Curve("curve1", 2);
      _curve.Material = _resourceManager.GetMaterial("lineMaterial");
      _curve.Configure(_renderer);
      _splines.Add(new SplineViewModel(new CatmullRom()));
      _splines.Add(new SplineViewModel(new Bezier()));
      SelectedSpline = _splines.First();
      _points = new Points("points", 5);
      _points.Material = _resourceManager.GetMaterial("pointMaterial");
      _points.Configure(_renderer);
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
      _points.Render(_renderer, Matrix4.Identity);
    }

    public void Update(double deltaTime)
    {

    }

    public void MouseDown(ControllerMouseButton button, int x, int y)
    {
      AddCurvePoint(x, y);
    }

    private void AddCurvePoint(int x, int y)
    {
      float adjustedX = (2.0f*(float) x/(float) _width) - 1;
      float adjustedY = (2.0f*(float) (_height - y)/(float) _height) - 1;
      var mousePoint = new Vector3(adjustedX, adjustedY, 0);
      foreach (var spline in Splines)
      {
        spline.Spline.AddPoint(mousePoint);
      }
      _points.AddPoint(mousePoint);
      if (_firstPoint)
      {
        _points.Configure(_renderer);
        _firstPoint = false;
      }
      else
        _points.Update(_renderer);

      UpdateCurve();
    }

    private void UpdateCurve()
    {
      if (SelectedSpline.Spline.Points.Count >= 4)
      {
        _curve.Clear();
        var points = SelectedSpline.Spline.Interpolate(_step);
        foreach (var point in points)
        {
          _curve.AddPoint(point);
        }
        if (_firstCurve)
        {
          _curve.Configure(_renderer);
          _firstCurve = false;
        }
        else
          _curve.Update(_renderer);
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
      if (key == 'r')
      {
        _curve.Clear();
        _points.Clear();
        foreach (var spline in Splines)
        {
          spline.Spline.Clear();
        }
      }
    }

    public void UpdateSize(double width, double height)
    {
      _width = width;
      _height = height;
    }
  }
}
