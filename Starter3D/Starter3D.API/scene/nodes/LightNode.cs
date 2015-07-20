using System.Globalization;
using OpenTK.Graphics;

namespace Starter3D
{
  public abstract class LightNode : BaseSceneNode
  {
    private Color4 _color;

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

    public override void Load(IDataNode dataNode)
    {
      float r = float.Parse(dataNode.ReadParameter("r"));
      float g = float.Parse(dataNode.ReadParameter("g"));
      float b = float.Parse(dataNode.ReadParameter("b"));
      Init(new Color4(r,g,b,1));
    }

    public override void Save(IDataNode dataNode)
    {
      dataNode.WriteParameter("r", Color.R.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("g", Color.G.ToString(CultureInfo.InvariantCulture));
      dataNode.WriteParameter("b", Color.B.ToString(CultureInfo.InvariantCulture));
    }
    
  }
}