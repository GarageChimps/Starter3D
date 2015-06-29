using ThreeAPI.utils;

namespace ThreeAPI.geometry.factories
{
  public interface IMeshFactory
  {
    IMesh CreateMesh(FileType fileType);
  }
}