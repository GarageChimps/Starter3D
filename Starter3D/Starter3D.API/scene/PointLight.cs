using System.Globalization;
using OpenTK;
using OpenTK.Graphics;

namespace ThreeAPI.scene
{
  public class PointLight : LightNode
  {
    private Vector3 _position;

    public Vector3 Position
    {
      get { return _position; }
    }

    public PointLight(Color4 color, Vector3 position)
      : base(color)
    {
      Init(position);
    }

    public PointLight()
    {
      
    }

    private void Init(Vector3 position)
    {
      _position = position;
    }

    public override void Load(IDataNode dataNode)
    {
      base.Load(dataNode);
      float x = float.Parse(dataNode.ReadParameter("x"));
      float y = float.Parse(dataNode.ReadParameter("y"));
      float z = float.Parse(dataNode.ReadParameter("z"));
      Init(new Vector3(x,y,z));
    }

    public override void Save(IDataNode dataNode)
    {
      base.Save(dataNode);
      dataNode.WriteParameter("x", Position.X.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("y", Position.Y.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("z", Position.Z.ToString(CultureInfo.InvariantCulture));
    }
  }
}