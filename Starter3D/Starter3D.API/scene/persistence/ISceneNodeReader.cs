using ThreeAPI.scene.nodes;

namespace ThreeAPI.scene.persistence
{
  public interface ISceneNodeReader
  {
    ISceneNode Read(string filePath);
  }
}