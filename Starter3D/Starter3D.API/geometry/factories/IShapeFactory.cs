using ThreeAPI.utils;

namespace ThreeAPI.geometry.factories
{
  public interface IShapeFactory
  {
    IShape CreateShape(ShapeType type, FileType fileType);
  }
}