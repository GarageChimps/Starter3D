using ThreeAPI.scene.nodes;

namespace ThreeAPI.scene.persistence
{
  public interface IXmlDataNodeReader
  {
    ISceneNode Read(string filePath);
  }
}