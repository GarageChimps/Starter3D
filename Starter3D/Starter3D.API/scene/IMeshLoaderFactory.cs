using System;

namespace ThreeAPI.scene
{
 
  public interface IMeshLoaderFactory
  {
    IMeshLoader CreateMeshLoader(FileType fileType);
  }

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
        default:
          throw new ArgumentOutOfRangeException("fileType");
      }
    }
  }
}