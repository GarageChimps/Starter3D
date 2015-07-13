using Starter3D.API.scene.nodes;

namespace Starter3D.API.scene.persistence
{
  public interface ISceneDataNode
  {
    ISceneNode Load();
    void WriteParameter(string parameterName, string parameterValue);
    bool HasParameter(string parameterName);
    string ReadParameter(string parameterName);
  }
}