using System.Globalization;
using OpenTK;
using OpenTK.Graphics;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class DirectionalLight : LightNode
  {
    private Vector3 _direction;

    public Vector3 Direction
    {
      get { return _direction; }
    }

    public DirectionalLight(Color4 color, Vector3 direction)
      : base(color)
    {
      Init(direction);
    }

    public DirectionalLight()
    {
      
    }

    private void Init(Vector3 direction)
    {
      _direction = direction;
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      base.Load(sceneDataNode);
      float x = float.Parse(sceneDataNode.ReadParameter("x"));
      float y = float.Parse(sceneDataNode.ReadParameter("y"));
      float z = float.Parse(sceneDataNode.ReadParameter("z"));
      Init(new Vector3(x,y,z).Normalized());
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      base.Save(sceneDataNode);
      sceneDataNode.WriteParameter("x", Direction.X.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("y", Direction.Y.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("z", Direction.Z.ToString(CultureInfo.InvariantCulture));
    }
  }
}