using Starter3D.API.utils;

namespace Starter3D.API.geometry.factories
{
  public interface IMeshFactory
  {
    IMesh CreateMesh(FileType fileType, string name);
  }
}