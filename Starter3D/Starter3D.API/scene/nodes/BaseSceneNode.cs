using System;
using System.Collections.Generic;

using OpenTK;

namespace Starter3D
{
  public class BaseSceneNode : ISceneNode
  {
    protected List<ISceneNode> _children = new List<ISceneNode>();
    protected ISceneNode _parent;
    protected Vector3 _position;
    protected Vector3 _rotation;

    public Vector3 Position {
      get { return _position; }
    }

    public Vector3 Rotation {
      get { return _rotation; }
    }

    public virtual Matrix4 Transform
    {
      get { return Matrix4.Identity; }
    }

    public IEnumerable<ISceneNode> Children
    {
      get { return _children; }
    }

    public ISceneNode Parent
    {
      get { return _parent; }
      set { _parent = value; }
    }

    public void Traverse(Action<ISceneNode> visit, Action<ISceneNode> finish = null){
      visit (this);
      foreach (ISceneNode node in Children) {
        node.Traverse (visit, finish);
      }
      if (finish != null)
        finish (this);
    }

    public Matrix4 ComposeTransform()
    {
      var transform = Transform;
      if (_parent != null)
        transform = _parent.ComposeTransform() * transform;
      return transform;
    }

    public void AddChild(ISceneNode child)
    {
      _children.Add(child);
      child.Parent = this;
    }

    public void RemoveChild(ISceneNode child)
    {
      _children.Remove(child);
      child.Parent = null;
    }

    public virtual void Load(IDataNode dataNode)
    {

    }

    public virtual void Save(IDataNode dataNode)
    {

    }

    public IEnumerable<T> GetNodes<T>() where T : class, ISceneNode
    {
      var elements = new List<T>();
      if (this.GetType().IsSubclassOf(typeof(T)))
        elements.Add(this as T);
      foreach (var child in Children)
      {
        var childElements = child.GetNodes<T>();
        elements.AddRange(childElements);
      }
      return elements;
    }

  }
}