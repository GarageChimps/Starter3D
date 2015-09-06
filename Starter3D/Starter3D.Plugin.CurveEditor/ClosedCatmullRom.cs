using System.Collections.Generic;
using OpenTK;

namespace Starter3D.Plugin.CurveEditor
{
  public class ClosedCatmullRom : CatmullRom
  {
    public override List<Vector3> Interpolate(float step)
    {
      var interpolated = new List<Vector3>();
      if (!Points[0].Equals(Points[Points.Count - 1]))
        Points.Add(Points[0]);

      var firstControlPoint = 0.5f*Points[0] + 0.5f*Points[Points.Count-2];
      var lastControlPoint = 2 * Points[1] - Points[0];
      Points.Add(lastControlPoint);
      var newPoints = new List<Vector3>();
      newPoints.Add(firstControlPoint);
      newPoints.AddRange(Points);
      for (int n = 0; n < Points.Count - 3; n++)
      {
        var controlPoints = new List<Vector3>() { newPoints[n], newPoints[n + 1], newPoints[n + 2], newPoints[n + 3] };
        for (float t = 0; t <= 1; t += step)
        {
          var point = BlendPoints(controlPoints, t);
          interpolated.Add(point);
        }
      }
      return interpolated;
    }
  }
}