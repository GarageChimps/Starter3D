using System.Collections.Generic;
using OpenTK;

namespace Starter3D.Plugin.CurveEditor
{
  public interface ISpline
  {
    string Name { get; }
    List<Vector3> Points { get; }
    void AddPoint(Vector3 point);
    void AddPoints(List<Vector3> points);
    List<Vector3> Interpolate(float step);
    void Clear();
  }
}