using ThreeAPI.scene.utils;

namespace ThreeAPI.scene.geometry.factories
{
  public interface IMeshFactory
  {
    IMesh CreateMesh(FileType fileType);
  }
}