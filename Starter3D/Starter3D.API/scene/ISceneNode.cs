using System.Collections.Generic;
using OpenTK;

namespace ThreeAPI.scene
{
  public enum SceneNodeType
  {
    Scene = 0,
    Translate,
    Rotate,
    Scale,
    Shape
  }

  public interface ISceneNode
  {
    IEnumerable<ISceneNode> Children { get; }
    ISceneNode Parent { get; set; }
    Matrix4 Transform { get;  }
    Matrix4 ComposeTransform();
    void AddChild(ISceneNode child);
    void RemoveChild(ISceneNode child);
    void Load(IDataNode dataNode);
    void Save(IDataNode dataNode);
  }
}