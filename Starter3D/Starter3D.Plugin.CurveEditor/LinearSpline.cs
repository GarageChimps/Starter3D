using System.Collections.Generic;
using OpenTK;

namespace Starter3D.Plugin.CurveEditor
{
  public class LinearSpline : Spline
  {
    public LinearSpline()
      : base() 
    {
      _basisMatrix.M11 = 1;
      _basisMatrix.M12 = 0;
      _basisMatrix.M21 = -1;
      _basisMatrix.M22 = 1;
    }

    protected override Vector3 BlendPoints(List<Vector3> points, float t)
    {
      var blendedPoint = FirstBlendingFunction(t) * points[0] + SecondBlendingFunction(t) * points[1];
      return blendedPoint;
    }

    protected float FirstBlendingFunction(float t)
    {
      return _basisMatrix.M11 + _basisMatrix.M21 * t;
    }

    protected float SecondBlendingFunction(float t)
    {
      return _basisMatrix.M12 + _basisMatrix.M22 * t;
    }

       

    public override List<Vector3> Interpolate(float step)
    {
      var interpolated = new List<Vector3>();

      for (int n = 1; n < Points.Count - 2; n++)
      {
        var controlPoints = new List<Vector3>() { Points[n], Points[n + 1]};
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