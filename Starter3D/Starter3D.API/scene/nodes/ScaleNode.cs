using System.Globalization;
using OpenTK;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class ScaleNode : TransformNode
  {
    private Vector3 _scaling = new Vector3();

    public float X
    {
      get { return _scaling.X; }
    }

    public float Y
    {
      get { return _scaling.Y; }
    }

    public float Z
    {
      get { return _scaling.Z; }
    }

    public Vector3 Scaling
    {
      get { return _scaling; }
    }

    public ScaleNode()
    {
      
    }

    public ScaleNode(float x, float y, float z)
    {
      Init(x, y, z);
    }

   

    private void Init(float x, float y, float z)
    {
      _scaling.X = x;
      _scaling.Y = y;
      _scaling.Z = z;
      _transform = Matrix4.CreateScale(_scaling);
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      float x = sceneDataNode.ReadFloatParameter("x");
      float y = sceneDataNode.ReadFloatParameter("y");
      float z = sceneDataNode.ReadFloatParameter("z");
      Init(x, y, z);
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      sceneDataNode.WriteParameter("x", X.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("y", Y.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("z", Z.ToString(CultureInfo.InvariantCulture));
    }
  }
}