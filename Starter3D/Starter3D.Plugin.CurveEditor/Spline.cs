using System.Collections.Generic;
using OpenTK;

namespace Starter3D.Plugin.CurveEditor
{
  public abstract class Spline
  {
    protected Matrix4 _basisMatrix;
    protected List<Vector3> _points;

    protected Spline()
    {
      _points = new List<Vector3>();
      _basisMatrix = new Matrix4();
    }

    public List<Vector3> Points
    {
      get { return _points; }
    }

    public void AddPoint(Vector3 point)
    {
      _points.Add(point);
    }

    public void AddPoints(List<Vector3> points)
    {
      this._points.AddRange(points);
    }

    protected abstract Vector3 BlendPoints(List<Vector3> points, float t);

    public abstract List<Vector3> Interpolate(float step);

  }
}
