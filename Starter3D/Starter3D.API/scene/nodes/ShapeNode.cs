using System;
using System.IO;
using Starter3D.API.geometry;
using Starter3D.API.geometry.factories;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.API.scene.nodes
{
  public class ShapeNode : BaseSceneNode
  {
    private IShape _shape;
    private readonly IShapeFactory _shapeFactory;
    private readonly IResourceManager _resourceManager;

    public IShape Shape
    {
      get { return _shape; }
    }

    public ShapeNode(IShape shape, IShapeFactory shapeFactory, IResourceManager resourceManager)
      : this(shapeFactory, resourceManager)
    {
      _shape = shape;
    }

    public ShapeNode(IShapeFactory shapeFactory, IResourceManager resourceManager)
    {
      _shapeFactory = shapeFactory;
      _resourceManager = resourceManager;
    }

    public override void Load(IDataNode dataNode)
    {
      var shapeTypeString = dataNode.ReadParameter("shapeType");
      var filePath = dataNode.ReadParameter("filePath");
      var fileTypeString = Path.GetExtension(filePath).TrimStart('.');

      var shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), shapeTypeString);
      var fileType = (FileType) Enum.Parse(typeof (FileType), fileTypeString);
      _shape = _shapeFactory.CreateShape(shapeType, fileType);
      _shape.Load(filePath);

      var materialKey = dataNode.ReadParameter("material");
      _shape.Material = _resourceManager.GetMaterial(materialKey);
    }

    public override void ConfigureRenderer(IRenderer renderer)
    {
      _shape.ConfigureRenderer(renderer);
      var modelTransform = ComposeTransform();
      renderer.AddMatrixParameter("modelMatrix", modelTransform);
      base.ConfigureRenderer(renderer);
    }
  }
}