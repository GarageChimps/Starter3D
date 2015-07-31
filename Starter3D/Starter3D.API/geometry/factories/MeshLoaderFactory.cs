using System;
using Starter3D.API.geometry.loaders;
using Starter3D.API.utils;

namespace Starter3D.API.geometry.factories
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

    public IMeshLoader CreateMeshLoader(FileType fileType)
    {
      switch (fileType)
      {
        case FileType.obj:
          return new ObjMeshLoader(_vertexFactory, _faceFactory);
        case FileType.dxf:
          return new DxfMeshLoader(_vertexFactory, _faceFactory);
        default:
          throw new ArgumentOutOfRangeException("fileType");
      }
    }
  }
}