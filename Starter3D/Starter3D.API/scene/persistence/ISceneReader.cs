using Starter3D.API.scene.nodes;

namespace Starter3D.API.scene.persistence
{
  public interface ISceneReader
  {
    ISceneNode Read(string filePath);
  }
}