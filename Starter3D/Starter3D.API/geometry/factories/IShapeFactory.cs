using ThreeAPI.utils;

namespace Starter3D
{
  public interface IShapeFactory
  {
    IShape CreateShape(ShapeType type, FileType fileType);
  }
}