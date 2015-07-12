using System.Globalization;
using OpenTK;
using Starter3D.API.renderer;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public abstract class CameraNode : BaseSceneNode
  {
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


    protected CameraNode(float nearClip, float farClip, int order)
    {
      Init(nearClip, farClip, order);
    }

    protected CameraNode()
    {
      
    }

    
    private void Init(float nearClip, float farClip, int order)
    {
      _nearClip = nearClip;
      _farClip = farClip;
      _order = order;
    }

    public override void Load(IDataNode dataNode)
    {
      float nearClip = float.Parse(dataNode.ReadParameter("nearClip"));
      float farClip = float.Parse(dataNode.ReadParameter("farClip"));
      int order = 0;
      if (dataNode.HasParameter("order"))
        order = int.Parse(dataNode.ReadParameter("order"));
      Init(nearClip, farClip, order);
    }

    public override void Save(IDataNode dataNode)
    {
      dataNode.WriteParameter("nearClip", _nearClip.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("farClip", _farClip.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("order", _order.ToString(CultureInfo.InvariantCulture));
    }
    
    public Matrix4 RenderTransform()
    {
      var viewMatrix = _parent.ComposeTransform().Inverted();
      var projectionMatrix = CreateProjectionMatrix();
      return projectionMatrix * viewMatrix;
    }

    public override void ConfigureRenderer(IRenderer renderer)
    {
      var viewMatrix = _parent.ComposeTransform().Inverted();
      renderer.AddMatrixParameter("viewMatrix", viewMatrix);
      var projectionMatrix = CreateProjectionMatrix();
      renderer.AddMatrixParameter("projectionMatrix", projectionMatrix);

      base.ConfigureRenderer(renderer);
    }

    protected abstract Matrix4 CreateProjectionMatrix();
  }
}