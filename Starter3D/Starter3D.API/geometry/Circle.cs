using System;
using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.geometry
{
  public class Circle : Curve
  {
    private readonly Vector3 _center;
    private readonly float _radius;
    private readonly float _resolution;

    private readonly List<Vector3> _points = new List<Vector3>(); 

    public Circle(string name, Vector3 center, float radius, float resolution)
      : base(name)
    {
      _center = center;
      _radius = radius;
      _resolution = resolution;

      Init();
    }

    public List<Vector3> Points
    {
      get { return _points; }
    }

    private void Init()
    {
      for (float t = 0; t <= 2 * MathHelper.Pi; t += _resolution)
      {
        var point = _center + _radius * new Vector3((float)Math.Cos(t), (float)Math.Sin(t), 0);
        _points.Add(point);
        AddPoint(point);
      }
    }
  }
}