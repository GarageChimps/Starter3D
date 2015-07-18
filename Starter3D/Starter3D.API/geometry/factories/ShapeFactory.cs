using System;
using Starter3D.API.utils;

namespace Starter3D.API.geometry.factories
{
  public class ShapeFactory : IShapeFactory
  {
    private readonly IMeshFactory _meshFactory;

    public ShapeFactory(IMeshFactory meshFactory)
    {
      _meshFactory = meshFactory;
    }

    public IShape CreateShape(ShapeType shapeType, FileType fileType, string name)
    {
      switch (shapeType)
      {
        case ShapeType.Mesh:
          return _meshFactory.CreateMesh(fileType, name);
        case ShapeType.PointCloud:
          throw new NotImplementedException();
        case ShapeType.Curve:
          throw new NotImplementedException();
        default:
          throw new ArgumentOutOfRangeException("shapeType");
      }
      
    }
  }
}