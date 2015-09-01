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
    private IMaterial _baseMaterial;

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

      var materialKey = sceneDataNode.ReadParameter("selectionMaterial");
      if (_resourceManager.HasMaterial(materialKey))
        _selectionMaterial = _resourceManager.GetMaterial(materialKey);
      _baseMaterial = _shape.Material;
    }

    public void Select(Ray ray)
    {
      var inverseModelMatrix = ComposeTransform();
      var newPosition = Vector4.Transform(new Vector4(ray.Position, 1), inverseModelMatrix);
      var newDirection = Vector4.Transform(new Vector4(ray.Direction, 0), inverseModelMatrix);
      var newRay = new Ray(newPosition.Xyz, newDirection.Xyz.Normalized());
      if (_shape.Intersects(newRay))
        _shape.Material = _selectionMaterial;
      else
        _shape.Material = _baseMaterial;

    }
  }
}