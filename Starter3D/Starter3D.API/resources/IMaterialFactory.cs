namespace ThreeAPI.resources
{
  public interface IMaterialFactory
  {
    IMaterial CreateMaterial(string vertexShader, string fragmentShader);
  }
}