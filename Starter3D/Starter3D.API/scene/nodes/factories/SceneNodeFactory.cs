using System;
using ThreeAPI.scene.geometry.factories;
using ThreeAPI.scene.utils;

namespace ThreeAPI.scene.nodes.factories
{
  public class SceneNodeFactory : ISceneNodeFactory
  {
    private readonly IShapeFactory _shapeFactory;

    public SceneNodeFactory(IShapeFactory shapeFactory)
    {
      _shapeFactory = shapeFactory;
    }

    public ISceneNode CreateSceneNode(SceneNodeType type)
    {
      switch (type)
      {
        case SceneNodeType.Scene:
          return new BaseSceneNode();
        case SceneNodeType.Translate:
          return new TranslationNode();
        case SceneNodeType.Rotate:
          return new RotationNode();
        case SceneNodeType.Scale:
          return new ScaleNode();
        case SceneNodeType.Shape:
          return new ShapeNode(_shapeFactory);
        case SceneNodeType.PerspectiveCamera:
          return new PerspectiveCamera();
        case SceneNodeType.OrthographicCamera:
          return new OrtographicCamera();
        case SceneNodeType.PointLight:
          return new PointLight();
        case SceneNodeType.DirectionalLight:
          return new DirectionalLight();
        default:
          throw new ArgumentOutOfRangeException("type");
      }
    }
  }
}