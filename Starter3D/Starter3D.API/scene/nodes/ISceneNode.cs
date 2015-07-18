using System.Collections.Generic;
using OpenTK;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public interface ISceneNode : IRenderElement
  {
    IEnumerable<ISceneNode> Children { get; }
    ISceneNode Parent { get; set; }
    Matrix4 Transform { get;  }
    Matrix4 ComposeTransform();
    void AddChild(ISceneNode child);
    void RemoveChild(ISceneNode child);
    void Load(ISceneDataNode sceneDataNode);
    void Save(ISceneDataNode sceneDataNode);
    IEnumerable<T> GetNodes<T>() where T : class, ISceneNode;
    
  }
}