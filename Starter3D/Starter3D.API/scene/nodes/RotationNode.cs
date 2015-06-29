using System.Globalization;
using OpenTK;
using ThreeAPI.scene.persistence;
using ThreeAPI.scene.utils;

namespace ThreeAPI.scene.nodes
{
  public class RotationNode : TransformNode
  {
    private Vector3 _axis = new Vector3();
    private float _angle;

    public float X
    {
      get { return _axis.X; }
    }

    public float Y
    {
      get { return _axis.Y; }
    }

    public float Z
    {
      get { return _axis.Z; }
    }

    public float Angle
    {
      get { return _angle; }
    }

    public RotationNode()
    {
      
    }

    public RotationNode(float x, float y, float z, float angle)
    {
      Init(x, y, z, angle);
    }
    
    private void Init(float x, float y, float z, float angle)
    {
      _axis.X = x;
      _axis.Y = y;
      _axis.Z = z;
      _angle = angle;
      _transform = Matrix4.CreateFromAxisAngle(_axis, angle.ToRadians());
    }

    public override void Load(IDataNode dataNode)
    {
      float x = float.Parse(dataNode.ReadParameter("x"));
      float y = float.Parse(dataNode.ReadParameter("y"));
      float z = float.Parse(dataNode.ReadParameter("z"));
      float angle = float.Parse(dataNode.ReadParameter("angle"));
      Init(x, y, z, angle);
    }

    public override void Save(IDataNode dataNode)
    {
      dataNode.WriteParameter("x", X.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("y", Y.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("z", Z.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("angle", _angle.ToString(CultureInfo.InvariantCulture));
    }
  }
}