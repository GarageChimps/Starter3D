using System.Globalization;
using OpenTK.Graphics;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public abstract class LightNode : BaseSceneNode
  {
    protected Color4 _color;

    public Color4 Color
    {
      get { return _color; }
    }

    protected LightNode(Color4 color)
    {
      Init(color);
    }

    protected LightNode()
    {
    }

    private void Init(Color4 color)
    {
      _color = color;
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      float r = float.Parse(sceneDataNode.ReadParameter("r"));
      float g = float.Parse(sceneDataNode.ReadParameter("g"));
      float b = float.Parse(sceneDataNode.ReadParameter("b"));
      Init(new Color4(r,g,b,1));
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      sceneDataNode.WriteParameter("r", Color.R.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("g", Color.G.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("b", Color.B.ToString(CultureInfo.InvariantCulture));
    }

  }
}