using Starter3D.API.utils;

namespace Starter3D.API.geometry.factories
{
  public interface IShapeFactory
  {
    IShape CreateShape(ShapeType type, FileType fileType, string name);
  }
}