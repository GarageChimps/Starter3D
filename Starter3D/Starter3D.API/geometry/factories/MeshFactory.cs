using Starter3D.API.utils;

namespace Starter3D.API.geometry.factories
{
  public class MeshFactory : IMeshFactory
  {
    private readonly IMeshLoaderFactory _meshLoaderFactory;

    public MeshFactory(IMeshLoaderFactory meshLoaderFactory)
    {
      _meshLoaderFactory = meshLoaderFactory;
    }

    public IMesh CreateMesh(FileType fileType, string name)
    {
      return new Mesh(_meshLoaderFactory.CreateMeshLoader(fileType), name);
    }
  }
}