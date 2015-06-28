using System.Globalization;
using OpenTK;
using ThreeAPI.scene.persistence;

namespace ThreeAPI.scene.nodes
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

    public override void Load(IDataNode dataNode)
    {
      float x = float.Parse(dataNode.ReadParameter("x"));
      float y = float.Parse(dataNode.ReadParameter("y"));
      float z = float.Parse(dataNode.ReadParameter("z"));
      Init(x, y, z);
    }

    public override void Save(IDataNode dataNode)
    {
      dataNode.WriteParameter("x", X.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("y", Y.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("z", Z.ToString(CultureInfo.InvariantCulture));
    }
  }
}