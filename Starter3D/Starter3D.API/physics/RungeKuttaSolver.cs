using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.physics
{
  public class RungeKuttaSolver : ISolver
  {
    public void Update(List<IRigidSolid> physicObjects, IDictionary<IRigidSolid, Vector3> forcesMap, float timeStep)
    {
      foreach (var obj in physicObjects)
      {
        var force = forcesMap[obj];

        var p1 = obj.LinearMomentum;
        var v1 = obj.Velocity;

        var p2 = obj.LinearMomentum + (timeStep / 2.0f) * force;
        var v2 = p2 / obj.Mass;

        var p3 = obj.LinearMomentum + (timeStep / 2.0f) * force;
        var v3 = p3 / obj.Mass;

        var p4 = obj.LinearMomentum + (timeStep) * force;
        var v4 = p4 / obj.Mass;

        obj.Position += (timeStep/6.0f)*(v1 + 2*v2 + 2*v3 + v4);
        obj.LinearMomentum += timeStep * force;
      }
    }
  }
}