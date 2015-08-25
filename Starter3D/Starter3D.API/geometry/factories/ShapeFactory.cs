using System;
using Starter3D.API.geometry.primitives;
using Starter3D.API.utils;

namespace Starter3D.API.geometry.factories
{
  public class ShapeFactory : IShapeFactory
  {
    private readonly IMeshFactory _meshFactory;
    private readonly IPrimitiveFactory _primitiveFactory;

    public ShapeFactory(IMeshFactory meshFactory, IPrimitiveFactory primitiveFactory)
    {
      _meshFactory = meshFactory;
      _primitiveFactory = primitiveFactory;
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

    public IShape CreateShape(PrimitiveType primitiveType, string name)
    {
      return _primitiveFactory.CreatePrimitive(primitiveType, name);
    }
  }
}