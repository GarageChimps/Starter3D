using System.Collections.Generic;
using OpenTK;

namespace Starter3D.Plugin.CurveEditor
{
  public abstract class CubicSpline : Spline
  {
    protected override Vector3 BlendPoints(List<Vector3> points, float t)
    {
      var blendedPoint = FirstBlendingFunction(t) * points[0] + SecondBlendingFunction(t) * points[1] +
                         ThirdBlendingFunction(t) * points[2] + FourthBlendingFunction(t) * points[3];
      return blendedPoint;
    }

    protected float FirstBlendingFunction(float t)
    {
      return _basisMatrix.M11 + _basisMatrix.M21 * t + _basisMatrix.M31 * t * t + _basisMatrix.M41 * t * t * t;
    }

    protected float SecondBlendingFunction(float t)
    {
      return _basisMatrix.M12 + _basisMatrix.M22 * t + _basisMatrix.M32 * t * t + _basisMatrix.M42 * t * t * t;
    }

    protected float ThirdBlendingFunction(float t)
    {
      return _basisMatrix.M13 + _basisMatrix.M23 * t + _basisMatrix.M33 * t * t + _basisMatrix.M43 * t * t * t;
    }

    protected float FourthBlendingFunction(float t)
    {
      return _basisMatrix.M14 + _basisMatrix.M24 * t + _basisMatrix.M34 * t * t + _basisMatrix.M44 * t * t * t;
    }

    
  }
}