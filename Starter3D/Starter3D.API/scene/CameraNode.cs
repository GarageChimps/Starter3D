using System.Globalization;
using OpenTK;

namespace ThreeAPI.scene
{
  public class CameraNode : BaseSceneNode
  {
    private float _fieldOfView;
    private float _nearClip;
    private float _farClip;
    private float _aspectRatio;

    public CameraNode(float fieldOfView, float nearClip, float farClip, float aspectRatio)
    {
      Init(fieldOfView, nearClip, farClip, aspectRatio);
    }

    private void Init(float fieldOfView, float nearClip, float farClip, float aspectRatio)
    {
      _fieldOfView = fieldOfView;
      _nearClip = nearClip;
      _farClip = farClip;
      _aspectRatio = aspectRatio;
    }

    public CameraNode()
    {

    }

    public override void Load(IDataNode dataNode)
    {
      float fov = float.Parse(dataNode.ReadParameter("fov")).ToRadians();
      float near = float.Parse(dataNode.ReadParameter("near"));
      float far = float.Parse(dataNode.ReadParameter("far"));
      float aspectRatio = float.Parse(dataNode.ReadParameter("aspect"));
      Init(fov, near, far, aspectRatio);
    }

    public override void Save(IDataNode dataNode)
    {
      dataNode.WriteParameter("fov", _fieldOfView.ToDegrees().ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("near", _nearClip.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("far", _farClip.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("aspect", _aspectRatio.ToString(CultureInfo.InvariantCulture));
    }

    public override Matrix4 ComposeTransform()
    {
      var viewMatrix = _parent.ComposeTransform().Inverted();
      var perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _farClip, _nearClip);
      return perspectiveMatrix * viewMatrix;
    }
  }
}