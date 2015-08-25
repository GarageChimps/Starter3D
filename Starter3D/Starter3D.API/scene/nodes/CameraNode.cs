using System.Globalization;
using OpenTK;
using Starter3D.API.renderer;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public abstract class CameraNode : BaseSceneNode
  {
    private Vector3 _position;
    private Vector3 _target;
    private Vector3 _up;
    
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

    public Vector3 Position
    {
      get { return _position; }
      set
      {
        _position = value;
        _isDirty = true;
      }
    }


    protected CameraNode(float nearClip, float farClip, int order, Vector3 position = default(Vector3),
      Vector3 target = default(Vector3), Vector3 up = default(Vector3))
    {
      Init(nearClip, farClip, order, position, target, up);
    }

    protected CameraNode()
    {

    }


    private void Init(float nearClip, float farClip, int order, Vector3 position = default(Vector3),
      Vector3 target = default(Vector3), Vector3 up = default(Vector3))
    {
      _nearClip = nearClip;
      _farClip = farClip;
      _order = order;
      _position = position;
      _target = target;
      _up = up;

    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      float nearClip = sceneDataNode.ReadFloatParameter("nearClip");
      float farClip = sceneDataNode.ReadFloatParameter("farClip");
      int order = 0;
      if (sceneDataNode.HasParameter("order"))
        order = int.Parse(sceneDataNode.ReadParameter("order"));
      var position = new Vector3();
      var target = new Vector3();
      var up = new Vector3();
      position = sceneDataNode.ReadVectorParameter("position");
      target = sceneDataNode.ReadVectorParameter("target");
      up = sceneDataNode.ReadVectorParameter("up");

      Init(nearClip, farClip, order, position, target, up);
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

    public override void Render(IRenderer renderer)
    {
      if (_isDirty)
      {
        Configure(renderer);
        _isDirty = false;
      }
    }

    public void Zoom(float delta)
    {
      var movementDirection = _target - _position;
      var zoom = delta * 0.05f * movementDirection.Length;
      _position += zoom * movementDirection.Normalized();
      _isDirty = true;
    }

    public void Drag(float deltaX, float deltaY)
    {
      var movementDirection = _target - _position;
      var leftVector = Vector3.Cross(_up, movementDirection);
      leftVector = leftVector.Normalized();
      var upMovementDirection = Vector3.Cross(movementDirection, leftVector);
      upMovementDirection = upMovementDirection.Normalized();

      deltaX = deltaX * 0.001f * movementDirection.Length;
      deltaY = deltaY * 0.001f * movementDirection.Length;

      _position += leftVector * deltaX;
      _target += leftVector * deltaX;

      _position += upMovementDirection * deltaY;
      _target += upMovementDirection * deltaY;
      _isDirty = true;
    }

    public void Orbit(float deltaX, float deltaY)
    {
      var movementDirection = _target - _position;
      var leftVector = Vector3.Cross(_up, movementDirection);
      leftVector = leftVector.Normalized();
      var upMovementDirection = Vector3.Cross(movementDirection, leftVector);
      upMovementDirection = upMovementDirection.Normalized();

      deltaX = -deltaX * 0.01f;
      deltaY = deltaY * 0.01f;

      var vectorToRotate = -movementDirection;
      var pitchQuat = Quaternion.FromAxisAngle(leftVector, deltaY);
      var pitchRotatedVector = Vector3.Transform(vectorToRotate, pitchQuat);
      var yawQuat = Quaternion.FromAxisAngle(upMovementDirection, deltaX);
      var yawRotatedVector = Vector3.Transform(pitchRotatedVector, yawQuat);
      _position = _target + yawRotatedVector;
      _isDirty = true;
    }

    private Vector3 GetPosition()
    {
      return _position;
    }

    private Matrix4 GetViewMatrix()
    {
      return Matrix4.LookAt(_position, _target, _up);
    }

    protected abstract Matrix4 CreateProjectionMatrix();
  }
}