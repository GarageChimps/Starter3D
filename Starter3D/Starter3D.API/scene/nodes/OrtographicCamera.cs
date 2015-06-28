using System.Globalization;
using OpenTK;
using ThreeAPI.scene.persistence;

namespace ThreeAPI.scene.nodes
{
  public class OrtographicCamera : CameraNode
  {
    private float _width;
    private float _height;

    public float Width
    {
      get { return _width; }
    }

    public float Height
    {
      get { return _height; }
    }

    public OrtographicCamera(float nearClip, float farClip, int order, float width, float height)
      : base(nearClip, farClip, order)
    {
      Init(width, height);
    }

    public OrtographicCamera()
    {

    }

    private void Init(float width, float height)
    {
      _width = width;
      _height = height;
    }

    public override void Load(IDataNode dataNode)
    {
      base.Load(dataNode);
      float width = float.Parse(dataNode.ReadParameter("width"));
      float height = float.Parse(dataNode.ReadParameter("height"));
      Init(width, height);
    }

    public override void Save(IDataNode dataNode)
    {
      base.Save(dataNode);
      dataNode.WriteParameter("width", _width.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("height", _height.ToString(CultureInfo.InvariantCulture));
    }

    protected override Matrix4 CreateProjectionMatrix()
    {
      return Matrix4.CreateOrthographic(_width, _height, NearClip, FarClip);
    }
  }
}