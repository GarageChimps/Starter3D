﻿using System.Collections.Generic;
using OpenTK;
using Starter3D.API.renderer;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class BaseSceneNode : ISceneNode
  {
    protected List<ISceneNode> _children = new List<ISceneNode>();
    protected ISceneNode _parent;
    protected bool _isDirty = true;

    protected virtual Matrix4 Transform { get { return Matrix4.Identity; } }

    public IEnumerable<ISceneNode> Children
    {
      get { return _children; }
    }

    public ISceneNode Parent
    {
      get { return _parent; }
      set { _parent = value; }
    }

    public Matrix4 ComposeTransform()
    {
      var transform = Transform;
      if (_parent != null)
        transform = transform * _parent.ComposeTransform();
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

    public virtual void Load(ISceneDataNode sceneDataNode)
    {

    }

    public virtual void Save(ISceneDataNode sceneDataNode)
    {

    }

    public IEnumerable<T> GetNodes<T>() where T : class, ISceneNode
    {
      var elements = new List<T>();
      if (this.GetType() == typeof(T))
        elements.Add(this as T);
      if (this.GetType().IsSubclassOf(typeof(T)))
        elements.Add(this as T);
      foreach (var child in Children)
      {
        var childElements = child.GetNodes<T>();
        elements.AddRange(childElements);
      }
      return elements;
    }

    public virtual void Configure(IRenderer renderer)
    {

    }

    public virtual void Render(IRenderer renderer)
    {

    }
  }
}