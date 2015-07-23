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
      if (sceneDataNode.HasParameter("tx"))
      {
        float x = float.Parse(sceneDataNode.ReadParameter("tx"));
        float y = float.Parse(sceneDataNode.ReadParameter("ty"));
        float z = float.Parse(sceneDataNode.ReadParameter("tz"));
        Init(new Vector3(x, y, z));
      }
    }

    public override void Configure(IRenderer renderer)
    {
      renderer.SetVectorParameter("pointLights[" + _index + "].Position", GetPosition());
      renderer.SetVectorParameter("pointLights[" + _index + "].Color", new Vector3(_color.R, _color.G, _color.B));
    }

   
  }
}