using ThreeAPI.utils;

namespace ThreeAPI.geometry.factories
{
  public class MeshFactory : IMeshFactory
  {
    private readonly IMeshLoaderFactory _meshLoaderFactory;

    public MeshFactory(IMeshLoaderFactory meshLoaderFactory)
    {
      _meshLoaderFactory = meshLoaderFactory;
    }

    public IMesh CreateMesh(FileType fileType)
    {
      return new Mesh(_meshLoaderFactory.CreateMeshLoader(fileType));
    }
  }
}