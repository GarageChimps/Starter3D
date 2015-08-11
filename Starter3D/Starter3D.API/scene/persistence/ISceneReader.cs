namespace Starter3D.API.scene.persistence
{
  public interface ISceneReader
  {
    IScene Read(string filePath);
  }
}