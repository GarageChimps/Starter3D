using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.physics
{
  public class EulerCromerSolver : ISolver
  {
    public void Update(List<IRigidSolid> physicObjects, IDictionary<IRigidSolid, Vector3> forcesMap, float timeStep)
    {
      foreach (var obj in physicObjects)
      {
        var force = forcesMap[obj];
        obj.LinearMomentum += timeStep * force;
        obj.Position += timeStep * obj.Velocity;
      }
    }
  }
}