using System.Collections.Generic;
using OpenTK;

namespace Starter3D.Plugin.CurveEditor
{
  class Bezier : CubicSpline
  {
    public Bezier()
      : base() 
    {
      _basisMatrix.M11 = 1;
      _basisMatrix.M12 = 0;
      _basisMatrix.M13 = 0;
      _basisMatrix.M14 = 0;
      _basisMatrix.M21 = -3;
      _basisMatrix.M22 = 3;
      _basisMatrix.M23 = 0;
      _basisMatrix.M24 = 0;
      _basisMatrix.M31 = 3;
      _basisMatrix.M32 = -6;
      _basisMatrix.M33 = 3;
      _basisMatrix.M34 = 0;
      _basisMatrix.M41 = -1;
      _basisMatrix.M42 = 3;
      _basisMatrix.M43 = -3;
      _basisMatrix.M44 = 1;
    }

    public override List<Vector3> Interpolate(float step)
    {
      var interpolated = new List<Vector3>();

      for (int n = 0; n < Points.Count - 3; n+=3)
      {
        var controlPoints = new List<Vector3>() { Points[n], Points[n + 1], Points[n + 2], Points[n + 3] };
        for (float t = 0; t < 1; t += step)
        {
          var point = BlendPoints(controlPoints, t);
          interpolated.Add(point);
        }
        var lastPoint = BlendPoints(controlPoints, 1);
        interpolated.Add(lastPoint);
      }
      return interpolated;
    }
  }
}