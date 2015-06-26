using System.Collections.Generic;
using OpenTK;

namespace ThreeAPI.scene
{
  public class BaseSceneNode : ISceneNode
  {
    protected List<ISceneNode> _children = new List<ISceneNode>();
    protected ISceneNode _parent;

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

    public virtual Matrix4 ComposeTransform()
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



  }
}