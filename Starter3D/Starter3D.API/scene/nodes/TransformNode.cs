using OpenTK;

namespace Starter3D
{
  public abstract class TransformNode : BaseSceneNode
  {
    protected Matrix4 _transform;

    public override Matrix4 Transform
    {
      get { return _transform; }
    }
  }
}