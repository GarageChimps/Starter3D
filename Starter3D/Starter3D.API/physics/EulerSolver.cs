using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.physics
{
  public class EulerSolver : ISolver
  {
    public void Update(List<IRigidSolid> physicObjects, IDictionary<IRigidSolid, Vector3> forcesMap, float timeStep)
    {
      foreach (var obj in physicObjects)
      {
        var force = forcesMap[obj];
        obj.Position += timeStep * obj.Velocity;
        obj.LinearMomentum += timeStep * force;
      }
    }
  }
}