using ThreeAPI.utils;

namespace Starter3D
{
 
  public interface IMeshLoaderFactory
  {
    IGeometryLoader CreateMeshLoader(FileType fileType);
  }
}