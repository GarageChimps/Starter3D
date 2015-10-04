using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.physics
{
  public interface ISolver
  {
    void Update(List<IRigidSolid> physicObjects, IDictionary<IRigidSolid, Vector3> forcesMap, float timeStep);
  }
}