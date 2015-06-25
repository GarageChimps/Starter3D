using System;
using System.IO;

namespace ThreeAPI.scene
{
  public class ShapeNode : BaseSceneNode
  {
    private readonly IShape _shape;
    private readonly IShapeFactory _shapeFactory;

    public IShape Shape
    {
      get { return _shape; }
    }

    public ShapeNode(IShape shape, IShapeFactory shapeFactory)
      : this(shapeFactory)
    {
      _shape = shape;
    }

    public ShapeNode(IShapeFactory shapeFactory)
    {
      _shapeFactory = shapeFactory;
    }

    public override void Load(IDataNode dataNode)
    {
      var shapeTypeString = dataNode.ReadParameter("shapeType");
      var filePath = dataNode.ReadParameter("filePath");
      var fileTypeString = Path.GetExtension(filePath);
      

      var shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), shapeTypeString);
      var fileType = (FileType) Enum.Parse(typeof (FileType), fileTypeString);
      var shape = _shapeFactory.CreateShape(shapeType, fileType);
      shape.Load(filePath);
    }
  }
}