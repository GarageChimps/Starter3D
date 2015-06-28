using ThreeAPI.scene.nodes;

namespace ThreeAPI.scene.persistence
{
  public interface IDataNode
  {
    ISceneNode Load();
    void WriteParameter(string parameterName, string parameterValue);
    bool HasParameter(string parameterName);
    string ReadParameter(string parameterName);
  }
}