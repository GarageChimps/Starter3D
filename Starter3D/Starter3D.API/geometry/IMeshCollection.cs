using OpenTK;

namespace Starter3D.API.geometry
{
  public interface IMeshCollection : IShape
  {
    void AddInstance(Matrix4 instanceMatrix);
  }
}