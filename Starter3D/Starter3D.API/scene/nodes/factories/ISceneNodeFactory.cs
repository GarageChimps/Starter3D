using ThreeAPI.utils;

namespace ThreeAPI.scene.nodes.factories
{
  public interface ISceneNodeFactory
  {
    ISceneNode CreateSceneNode(SceneNodeType type);
  }
}