namespace Starter3D.API.utils
{
  public interface IConfiguration
  {
    bool HasParameter(string parameterName);
    string GetParameter(string parameterName);
  }
}
