using System;
using System.Collections.Generic;

using OpenTK;

namespace Starter3D
{
  public interface ISceneNode
  {
    IEnumerable<ISceneNode> Children { get; }
    ISceneNode Parent { get; set; }
    Matrix4 Transform { get;  }
    void Traverse(Action<ISceneNode> visit, Action<ISceneNode> finish);
    Matrix4 ComposeTransform();
    void AddChild(ISceneNode child);
    void RemoveChild(ISceneNode child);
    void Load(IDataNode dataNode);
    void Save(IDataNode dataNode);
    IEnumerable<T> GetNodes<T>() where T : class, ISceneNode;
  }
}