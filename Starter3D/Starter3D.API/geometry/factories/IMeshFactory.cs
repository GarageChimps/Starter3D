using ThreeAPI.utils;

namespace Starter3D
{
  public interface IMeshFactory
  {
    IGeometry CreateMesh(FileType fileType);
  }
}