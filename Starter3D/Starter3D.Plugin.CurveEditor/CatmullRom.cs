using System.Collections.Generic;
using OpenTK;

namespace Starter3D.Plugin.CurveEditor
{
  public class CatmullRom : CubicSpline
  {
    public CatmullRom() : base()
    {
      _name = "Catmull Rom";
      _basisMatrix.M11 = 0;
      _basisMatrix.M12 = 1;
      _basisMatrix.M13 = 0;
      _basisMatrix.M14 = 0;
      _basisMatrix.M21 = -0.5f;
      _basisMatrix.M22 = 0;
      _basisMatrix.M23 = 0.5f;
      _basisMatrix.M24 = 0;
      _basisMatrix.M31 = 1;
      _basisMatrix.M32 = -2.5f;
      _basisMatrix.M33 = 2;
      _basisMatrix.M34 = -0.5f;
      _basisMatrix.M41 = -0.5f;
      _basisMatrix.M42 = 1.5f;
      _basisMatrix.M43 = -1.5f;
      _basisMatrix.M44 = 0.5f;
    }

    public override List<Vector3> Interpolate(float step)
    {
      var interpolated = new List<Vector3>();

      for (int n = 0; n < Points.Count - 3; n++)
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