namespace ThreeAPI.scene
{
  public interface IShapeFactory
  {
    IShape CreateShape(ShapeType type, FileType fileType);
  }
}