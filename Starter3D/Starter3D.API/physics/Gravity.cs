using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.physics
{
  public class Gravity : IForce
  {
    private const float GravityConstant = 1;//6.674E-11f;

    private readonly IRigidSolid _gravitiyObject;

    public Gravity(IRigidSolid gravitiyObject)
    {
      _gravitiyObject = gravitiyObject;
    }

    public IDictionary<IRigidSolid, Vector3> CalculateForce(IEnumerable<IRigidSolid> physicsObject)
    {
      var forceMap = new Dictionary<IRigidSolid, Vector3>();
      foreach (var rigidSolidB in physicsObject)
      {
        if (rigidSolidB != _gravitiyObject)
        {
          var d = (rigidSolidB.Position - _gravitiyObject.Position).Length;
          var normalDirection = (rigidSolidB.Position - _gravitiyObject.Position).Normalized();
          var force = -GravityConstant * _gravitiyObject.Mass * rigidSolidB.Mass * (1.0f / (d * d)) * normalDirection;
          forceMap[rigidSolidB] = force;
        }
      }
      return forceMap;
    }

  }
}