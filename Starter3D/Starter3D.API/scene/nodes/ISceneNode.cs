using System.Collections.Generic;
using OpenTK;
using ThreeAPI.scene.persistence;

namespace ThreeAPI.scene.nodes
{
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
    IEnumerable<T> GetNodes<T>() where T : class, ISceneNode;
  }
}