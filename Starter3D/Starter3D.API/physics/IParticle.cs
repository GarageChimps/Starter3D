using OpenTK;

namespace Starter3D.API.physics
{
  public interface IParticle
  {
    float Mass { get; }
    Vector3 Position { get; set; }
    Vector3 LinearMomentum { get; set; }
    Vector3 Velocity { get; }
  }
}
