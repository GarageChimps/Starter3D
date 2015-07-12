namespace Starter3D.API.resources
{
  public interface IMaterialFactory
  {
    IMaterial CreateMaterial(string vertexShader, string fragmentShader);
  }
}