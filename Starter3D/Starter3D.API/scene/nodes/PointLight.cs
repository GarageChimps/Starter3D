using OpenTK;
using OpenTK.Graphics;
using Starter3D.API.renderer;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class PointLight : LightNode
  {
    private Vector3 _position;

    public PointLight(Color4 color, Vector3 position = default(Vector3))
      : base(color)
    {
      Init(position);
    }

    public PointLight()
    {

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

    private void Init(Vector3 position)
    {
      _position = position;
    }

    public Vector3 GetPosition()
    {
      return _position;
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      base.Load(sceneDataNode);
      var position = sceneDataNode.ReadVectorParameter("position");
      Init(position);
    }

    public override void Configure(IRenderer renderer)
    {
      renderer.SetVectorArrayParameter("pointLightPositions", _index, GetPosition());
      renderer.SetVectorArrayParameter("pointLightColors", _index, new Vector3(_color.R, _color.G, _color.B));
    }
  }
}