using ThreeAPI.geometry.loaders;
using ThreeAPI.utils;

namespace ThreeAPI.geometry.factories
{
 
  public interface IMeshLoaderFactory
  {
    IMeshLoader CreateMeshLoader(FileType fileType);
  }
}