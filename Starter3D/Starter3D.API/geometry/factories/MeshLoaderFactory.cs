using System;
using ThreeAPI.utils;

namespace Starter3D
{
  public class MeshLoaderFactory : IMeshLoaderFactory
  {
    private readonly IVertexFactory _vertexFactory;
    private readonly IFaceFactory _faceFactory;

    public MeshLoaderFactory(IVertexFactory vertexFactory, IFaceFactory faceFactory)
    {
      _vertexFactory = vertexFactory;
      _faceFactory = faceFactory;
    }

    public IGeometryLoader CreateMeshLoader(FileType fileType)
    {
      switch (fileType)
      {
        case FileType.obj:
          return new ObjMeshLoader(_vertexFactory, _faceFactory);
        default:
          throw new ArgumentOutOfRangeException("fileType");
      }
    }
  }
}