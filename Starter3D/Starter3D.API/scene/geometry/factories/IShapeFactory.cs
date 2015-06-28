using ThreeAPI.scene.utils;

namespace ThreeAPI.scene.geometry.factories
{
  public interface IShapeFactory
  {
    IShape CreateShape(ShapeType type, FileType fileType);
  }
}