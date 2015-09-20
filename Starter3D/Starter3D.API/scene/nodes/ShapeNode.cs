using System;
using System.IO;
using System.Security.Cryptography;
using OpenTK;
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
    protected IShape _shape;
    private readonly IShapeFactory _shapeFactory;
    protected readonly IResourceManager _resourceManager;
    private Vector3 _position;
    private Quaternion _rotation;
    private Vector3 _scale;

    protected override Matrix4 Transform { get { return GetModelTransform(); } }

    public IShape Shape
    {
      get { return _shape; }
      set { _shape = value; }
    }

    public Quaternion Rotation
    {
      get { return _rotation; }
      set { _rotation = value; }
    }

    public Vector3 Position
    {
      get { return _position; }
      set { _position = value; }
    }

    public Vector3 Scale
    {
      get { return _scale; }
      set { _scale = value; }
    }

    public ShapeNode(IShape shape, IShapeFactory shapeFactory, IResourceManager resourceManager, Vector3 scale = default(Vector3), Vector3 position = default(Vector3), 
      Vector3 orientationAxis = default(Vector3), float orientationAngle = 0)
      : this(shapeFactory, resourceManager, scale, position, orientationAxis, orientationAngle)
    {
      _shape = shape;
    }

    public ShapeNode(IShapeFactory shapeFactory, IResourceManager resourceManager, Vector3 scale = default(Vector3), Vector3 position = default(Vector3),
      Vector3 orientationAxis = default(Vector3), float orientationAngle = 0)
    {
      _shapeFactory = shapeFactory;
      _resourceManager = resourceManager;
      Init(scale, position, orientationAxis, orientationAngle);
    }

    public ShapeNode Clone()
    {
      var cloneShape = _shape.Clone();
      var axisAngle = _rotation.ToAxisAngle();
      return new ShapeNode(cloneShape, _shapeFactory, _resourceManager, _scale, _position, new Vector3(axisAngle.X, axisAngle.Y, axisAngle.Z), axisAngle.W);
    }

    private void Init(Vector3 scale = default(Vector3), Vector3 position = default(Vector3),
      Vector3 orientationAxis = default(Vector3), float orientationAngle = 0)
    {
      _scale = scale;
      _position = position;
      _rotation = Quaternion.FromAxisAngle(orientationAxis, orientationAngle);
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      var scale = new Vector3(1,1,1);
      var position = new Vector3();
      var orientationAxis = new Vector3(0,0,1);
      var orientationAngle = 0.0f;
      if (sceneDataNode.HasParameter("scale"))
      {
        scale = sceneDataNode.ReadVectorParameter("scale");
      } 
      if (sceneDataNode.HasParameter("position"))
      {
        position = sceneDataNode.ReadVectorParameter("position");
      }
      if (sceneDataNode.HasParameter("orientationAxis"))
      {
        orientationAxis = sceneDataNode.ReadVectorParameter("orientationAxis");
        orientationAngle = sceneDataNode.ReadFloatParameter("angle");
      }
      Init(scale, position, orientationAxis, orientationAngle);

      var name = sceneDataNode.ReadParameter("shapeName");

      if (sceneDataNode.HasParameter("filePath"))
      {
        var shapeTypeString = sceneDataNode.ReadParameter("shapeType");
        var shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), shapeTypeString);
        var filePath = sceneDataNode.ReadParameter("filePath");
        var fileTypeString = Path.GetExtension(filePath).TrimStart('.');

        var fileType = (FileType) Enum.Parse(typeof (FileType), fileTypeString);
        _shape = _shapeFactory.CreateShape(shapeType, fileType, name);
        _shape.Load(filePath);
      }
      else
      {
        var primitveTypeString = sceneDataNode.ReadParameter("primitiveType");
        var primitveType = (PrimitiveType)Enum.Parse(typeof(PrimitiveType), primitveTypeString);
        _shape = _shapeFactory.CreateShape(primitveType, name);
      }

      var materialKey = sceneDataNode.ReadParameter("material");
      if (_resourceManager.HasMaterial(materialKey))
        _shape.Material = _resourceManager.GetMaterial(materialKey);
    }

    public override void Configure(IRenderer renderer)
    {
      _shape.Configure(renderer);
    }
    
    public override void Render(IRenderer renderer)
    {
      var modelTransform = ComposeTransform();
      _shape.Render(renderer, modelTransform);
    }

    private Matrix4 GetModelTransform()
    {
      var translation = Matrix4.CreateTranslation(_position);
      var rotation = Matrix4.CreateFromQuaternion(_rotation);
      var scale = Matrix4.CreateScale(_scale);
      var matrix =  scale * rotation * translation;
      return matrix;
    }
  }
}