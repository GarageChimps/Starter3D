using System.Globalization;
using OpenTK;
using OpenTK.Graphics;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class PointLight : LightNode
  {
    private Vector3 _position;

    public Vector3 Position
    {
      get { return _position; }
    }

    public PointLight(Color4 color, Vector3 position)
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
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      base.Load(sceneDataNode);
      float x = float.Parse(sceneDataNode.ReadParameter("x"));
      float y = float.Parse(sceneDataNode.ReadParameter("y"));
      float z = float.Parse(sceneDataNode.ReadParameter("z"));
      Init(new Vector3(x,y,z));
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      base.Save(sceneDataNode);
      sceneDataNode.WriteParameter("x", Position.X.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("y", Position.Y.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("z", Position.Z.ToString(CultureInfo.InvariantCulture));
    }
  }
}