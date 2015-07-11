using System;
using ThreeAPI.geometry.factories;
using ThreeAPI.resources;
using ThreeAPI.utils;

namespace ThreeAPI.scene.nodes.factories
{
  public class SceneNodeFactory : ISceneNodeFactory
  {
    private readonly IShapeFactory _shapeFactory;
    private readonly IResourceManager _resourceManager;

    public SceneNodeFactory(IShapeFactory shapeFactory, IResourceManager resourceManager)
    {
      if (shapeFactory == null) throw new ArgumentNullException("shapeFactory");
      if (resourceManager == null) throw new ArgumentNullException("resourceManager");
      _shapeFactory = shapeFactory;
      _resourceManager = resourceManager;
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
          return new ShapeNode(_shapeFactory, _resourceManager);
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