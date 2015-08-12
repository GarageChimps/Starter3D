using System;
using Starter3D.API.geometry.factories;
using Starter3D.API.resources;
using Starter3D.API.utils;

namespace Starter3D.API.scene.nodes.factories
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
        case SceneNodeType.AmbientLight:
          return new AmbientLight();
        default:
          throw new ArgumentOutOfRangeException("type");
      }
    }
  }
}