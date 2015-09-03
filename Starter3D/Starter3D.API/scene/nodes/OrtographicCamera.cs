using System.Globalization;
using OpenTK;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
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

    public override void Load(ISceneDataNode sceneDataNode)
    {
      base.Load(sceneDataNode);
      float width = sceneDataNode.ReadFloatParameter("width"); ;
      float height = sceneDataNode.ReadFloatParameter("height"); ;
      Init(width, height);
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      base.Save(sceneDataNode);
      sceneDataNode.WriteParameter("width", _width.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("height", _height.ToString(CultureInfo.InvariantCulture));
    }

    protected override Matrix4 CreateProjectionMatrix()
    {
      return Matrix4.CreateOrthographic(_width, _height, NearClip, FarClip);
    }
  }
}