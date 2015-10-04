using OpenTK;

namespace Starter3D.API.physics
{
  public interface IRigidSolid : IParticle
  {
    Quaternion Rotation { get; }
    Vector3 AngularMomentum { get; }
  }
}