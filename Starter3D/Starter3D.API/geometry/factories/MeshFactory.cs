using ThreeAPI.utils;

namespace Starter3D
{
  public class MeshFactory : IMeshFactory
  {
    private readonly IMeshLoaderFactory _meshLoaderFactory;

    public MeshFactory(IMeshLoaderFactory meshLoaderFactory)
    {
      _meshLoaderFactory = meshLoaderFactory;
    }

    public IGeometry CreateMesh(FileType fileType)
    {
      return new Geometry(_meshLoaderFactory.CreateMeshLoader(fileType));
    }
  }
}