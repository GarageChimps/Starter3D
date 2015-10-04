using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.physics
{
  public interface IPhysicsEngine
  {
    void AddObject(IRigidSolid obj);
    void AddForce(IForce force);
    void Update(float timeStep);
  }
}