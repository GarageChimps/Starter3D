
namespace Starter3D{

  public interface IDataNode
  {
    ISceneNode Load();
    void WriteParameter(string parameterName, string parameterValue);
    bool HasParameter(string parameterName);
    string ReadParameter(string parameterName);
  }
}