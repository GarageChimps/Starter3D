using OpenTK;
using Starter3D.API.geometry;
using Starter3D.API.geometry.factories;
using Starter3D.API.math;
using Starter3D.API.resources;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class InteractiveShapeNode : ShapeNode
  {
    private IMaterial _selectionMaterial;
    private IMaterial _highlightMaterial;
    private IMaterial _baseMaterial;

    private bool _isSelected;

    public InteractiveShapeNode(IShape shape, IShapeFactory shapeFactory, IResourceManager resourceManager, Vector3 scale = new Vector3(), Vector3 position = new Vector3(), Vector3 orientationAxis = new Vector3(), float orientationAngle = 0)
      : base(shape, shapeFactory, resourceManager, scale, position, orientationAxis, orientationAngle)
    {
    }

    public InteractiveShapeNode(IShapeFactory shapeFactory, IResourceManager resourceManager, Vector3 scale = new Vector3(), Vector3 position = new Vector3(), Vector3 orientationAxis = new Vector3(), float orientationAngle = 0)
      : base(shapeFactory, resourceManager, scale, position, orientationAxis, orientationAngle)
    {
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      base.Load(sceneDataNode);

      var selectionMaterialKey = sceneDataNode.ReadParameter("selectionMaterial");
      if (_resourceManager.HasMaterial(selectionMaterialKey))
        _selectionMaterial = _resourceManager.GetMaterial(selectionMaterialKey);
      var highlightMaterialKey = sceneDataNode.ReadParameter("highlightMaterial");
      if (_resourceManager.HasMaterial(highlightMaterialKey))
        _highlightMaterial = _resourceManager.GetMaterial(highlightMaterialKey);
      _baseMaterial = _shape.Material;
    }

    public bool TestIntersection(Ray ray)
    {
      var inverseModelMatrix = ComposeTransform().Inverted();
      var newPosition = Vector4.Transform(new Vector4(ray.Position, 1), inverseModelMatrix);
      var newDirection = Vector4.Transform(new Vector4(ray.Direction, 0), inverseModelMatrix);
      var newRay = new Ray(newPosition.Xyz, newDirection.Xyz.Normalized());
      var intersects = _shape.Intersects(newRay);
      if (intersects)
        _shape.Material = _highlightMaterial;
      else if (_isSelected)
        _shape.Material = _selectionMaterial;
      else
        _shape.Material = _baseMaterial;
      return intersects;
    }

    public void Select(bool isSelected)
    {
      _isSelected = isSelected;
      if (isSelected)
        _shape.Material = _selectionMaterial;
      else
        _shape.Material = _baseMaterial;
    }
  }
}