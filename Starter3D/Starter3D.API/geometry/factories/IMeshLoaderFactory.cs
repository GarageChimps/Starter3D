using Starter3D.API.geometry.loaders;
using Starter3D.API.utils;

namespace Starter3D.API.geometry.factories
{
 
  public interface IMeshLoaderFactory
  {
    IMeshLoader CreateMeshLoader(FileType fileType);
  }
}