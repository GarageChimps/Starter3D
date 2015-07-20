using ThreeAPI.utils;

namespace Starter3D
{
  public interface ISceneNodeFactory
  {
    ISceneNode CreateSceneNode(SceneNodeType type);
  }
}