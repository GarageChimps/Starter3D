using ThreeAPI.scene.geometry.loaders;
using ThreeAPI.scene.utils;

namespace ThreeAPI.scene.geometry.factories
{
 
  public interface IMeshLoaderFactory
  {
    IMeshLoader CreateMeshLoader(FileType fileType);
  }
}