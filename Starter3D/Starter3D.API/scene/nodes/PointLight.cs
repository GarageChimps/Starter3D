using OpenTK;
using OpenTK.Graphics;
using Starter3D.API.renderer;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class PointLight : LightNode
  {
    private bool _hasPosition;
    private Vector3 _position;

    public PointLight(Color4 color, Vector3 position)
      : base(color)
    {
      _position = position;
    }

    public PointLight()
    {
      
    }

    private void Init(Vector3 position)
    {
      _position = position;
      _hasPosition = true;
    }

    public Vector3 GetPosition()
    {
      if(_hasPosition)
        return _position;
      var transform = ComposeTransform();
      return transform.Row3.Xyz;
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      base.Load(sceneDataNode);
      //Light position can be confgured through the scene graph or directly as a parameter of the node
      if (sceneDataNode.HasParameter("x"))
      {
        float x = float.Parse(sceneDataNode.ReadParameter("x"));
        float y = float.Parse(sceneDataNode.ReadParameter("y"));
        float z = float.Parse(sceneDataNode.ReadParameter("z"));
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