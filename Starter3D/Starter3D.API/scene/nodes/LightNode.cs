using System.Globalization;
using OpenTK.Graphics;
using Starter3D.API.renderer;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public abstract class LightNode : BaseSceneNode
  {
    protected int _index;
    protected Color4 _color;

    public Color4 Color
    {
      get { return _color; }
      set
      {
        _color = value;
        _isDirty = true;
      }
    }

    public int Index
    {
      get { return _index; }
      set { _index = value; }
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

    public override void Render(IRenderer renderer)
    {
      if (_isDirty)
      {
        Configure(renderer);
        _isDirty = false;
      }
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      Init(sceneDataNode.ReadColorParameter("color"));
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      sceneDataNode.WriteParameter("r", Color.R.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("g", Color.G.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("b", Color.B.ToString(CultureInfo.InvariantCulture));
    }

  }
}