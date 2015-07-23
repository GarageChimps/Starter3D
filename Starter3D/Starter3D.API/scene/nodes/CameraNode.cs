using System.Globalization;
using OpenTK;
using Starter3D.API.renderer;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.API.scene.nodes
{
  public abstract class CameraNode : BaseSceneNode
  {
    private bool _hasTransform;
    private Vector3 _position;
    private Vector3 _orientationAxis;
    private float _orientationAngle;

    protected float _nearClip;
    protected float _farClip;
    protected int _order;

    public float NearClip
    {
      get { return _nearClip; }
    }

    public float FarClip
    {
      get { return _farClip; }
    }

    public int Order
    {
      get { return _order; }
    }


    protected CameraNode(float nearClip, float farClip, int order, Vector3 position = default(Vector3), 
      Vector3 orientationAxis = default(Vector3), float orientationAngle = 0)
    {
      Init(nearClip, farClip, order, position, orientationAxis, orientationAngle);
    }

    protected CameraNode()
    {

    }


    private void Init(float nearClip, float farClip, int order, Vector3 position = default(Vector3), 
      Vector3 orientationAxis = default(Vector3), float orientationAngle = 0)
    {
      _nearClip = nearClip;
      _farClip = farClip;
      _order = order;
      _position = position;
      _orientationAxis = orientationAxis;
      _orientationAngle = orientationAngle;

    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      float nearClip = float.Parse(sceneDataNode.ReadParameter("nearClip"));
      float farClip = float.Parse(sceneDataNode.ReadParameter("farClip"));
      int order = 0;
      if (sceneDataNode.HasParameter("order"))
        order = int.Parse(sceneDataNode.ReadParameter("order"));
      var position = new Vector3();
      var orientationAxis = new Vector3();
      var orientationAngle = 0.0f;
      if (sceneDataNode.HasParameter("tx"))
      {
        _hasTransform = true;
        float tx = float.Parse(sceneDataNode.ReadParameter("tx"));
        float ty = float.Parse(sceneDataNode.ReadParameter("ty"));
        float tz = float.Parse(sceneDataNode.ReadParameter("tz"));
        position = new Vector3(tx, ty, tz);
      }
      if (sceneDataNode.HasParameter("rx"))
      {
        _hasTransform = true;
        float rx = float.Parse(sceneDataNode.ReadParameter("rx"));
        float ry = float.Parse(sceneDataNode.ReadParameter("ry"));
        float rz = float.Parse(sceneDataNode.ReadParameter("rz"));
        orientationAxis = new Vector3(rx, ry, rz);
        orientationAngle = float.Parse(sceneDataNode.ReadParameter("angle"));
      }

      Init(nearClip, farClip, order, position, orientationAxis, orientationAngle);
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      sceneDataNode.WriteParameter("nearClip", _nearClip.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("farClip", _farClip.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("order", _order.ToString(CultureInfo.InvariantCulture));
    }


    public override void Configure(IRenderer renderer)
    {
      var viewMatrix = GetViewMatrix();
      var projectionMatrix = CreateProjectionMatrix();
      renderer.SetVectorParameter("cameraPosition", GetPosition());
      renderer.SetMatrixParameter("viewMatrix", viewMatrix);
      renderer.SetMatrixParameter("projectionMatrix", projectionMatrix);

    }

    public override void Update(IRenderer renderer)
    {
      Configure(renderer);
    }

    private Vector3 GetPosition()
    {
      if (_hasTransform)
        return _position;
      var transform = ComposeTransform();
      return transform.Row3.Xyz;
    }

    private Matrix4 GetViewMatrix()
    {
      if (_hasTransform)
      {
        var translation = Matrix4.CreateTranslation(_position);
        var rotation = Matrix4.CreateFromAxisAngle(_orientationAxis, _orientationAngle.ToRadians());
        var matrix = translation * rotation;
        return matrix.Inverted();
      }
      var transform = ComposeTransform();
      return transform.Inverted();
    }

    protected abstract Matrix4 CreateProjectionMatrix();
  }
}