using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.physics
{
  public interface IForce
  {
    IDictionary<IRigidSolid, Vector3> CalculateForce(IEnumerable<IRigidSolid> physicsObject);
  }
}