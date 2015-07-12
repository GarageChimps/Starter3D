namespace Starter3D.API.geometry.loaders
{
  public interface IMeshLoader
  {
    void Load(IMesh mesh, string filePath);
  }
}