using Starter3D.API.scene.nodes;

namespace Starter3D.API.scene.persistence
{
  public interface ISceneNodeReader
  {
    ISceneNode Read(string filePath);
  }
}