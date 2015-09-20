using OpenTK;

namespace Starter3D.API.geometry
{
  public interface IMeshCollection : IShape
  {
    void AddInstance(Vector3 instancePosition);
  }
}