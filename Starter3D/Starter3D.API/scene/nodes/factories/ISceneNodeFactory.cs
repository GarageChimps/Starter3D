using Starter3D.API.utils;

namespace Starter3D.API.scene.nodes.factories
{
  public interface ISceneNodeFactory
  {
    ISceneNode CreateSceneNode(SceneNodeType type);
  }
}