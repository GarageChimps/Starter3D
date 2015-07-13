using OpenTK;

namespace Starter3D.API.resources
{
  public interface IDataNode
  {
    void WriteParameter(string parameterName, string parameterValue);
    bool HasParameter(string parameterName);
    string ReadParameter(string parameterName);
    float ReadFloatParameter(string parameterName);
    Vector3 ReadVectorParameter(string parameterName);
  }
}