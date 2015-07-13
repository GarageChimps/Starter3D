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

    public override void Load(ISceneDataNode sceneDataNode)
    {
      float nearClip = float.Parse(sceneDataNode.ReadParameter("nearClip"));
      float farClip = float.Parse(sceneDataNode.ReadParameter("farClip"));
      int order = 0;
      if (sceneDataNode.HasParameter("order"))
        order = int.Parse(sceneDataNode.ReadParameter("order"));
      Init(nearClip, farClip, order);
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      sceneDataNode.WriteParameter("nearClip", _nearClip.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("farClip", _farClip.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("order", _order.ToString(CultureInfo.InvariantCulture));
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