using OpenTK;
using OpenTK.Graphics;
using Starter3D.API.renderer;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class PointLight : LightNode
  {
    private bool _hasTransform;
    private Vector3 _position;

    public PointLight(Color4 color, Vector3 position=default(Vector3))
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
      set { _position = value; }
    }

    private void Init(Vector3 position)
    {
      _position = position;
      _hasTransform = true;
    }

    public Vector3 GetPosition()
    {
      if(_hasTransform)
        return _position;
      var transform = ComposeTransform();
      return transform.Row3.Xyz;
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      base.Load(sceneDataNode);
      //Light position can be confgured through the scene graph or directly as a parameter of the node
      if (sceneDataNode.HasParameter("position"))
      {
        var position = sceneDataNode.ReadVectorParameter("position");
        Init(position);
      }
    }

    public override void Configure(IRenderer renderer)
    {
      renderer.SetVectorArrayParameter("pointLightPositions", _index, GetPosition());
      renderer.SetVectorArrayParameter("pointLightColors", _index, new Vector3(_color.R, _color.G, _color.B));
    }

    public override void Render(IRenderer renderer)
    {
      Configure(renderer);
    }
  }
}