using System;
using System.Globalization;
using OpenTK;
using OpenTK.Graphics;
using Starter3D.API.renderer;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class DirectionalLight : LightNode
  {
    private Vector3 _direction;

    public Vector3 Direction
    {
      get { return _direction; }
      set
      {
        _direction = value;
        _isDirty = true;
      }
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

    public override void Configure(IRenderer renderer)
    {
      renderer.SetVectorArrayParameter("directionalLightDirections", _index, _direction);
      renderer.SetVectorArrayParameter("directionalLightColors", _index, new Vector3(_color.R, _color.G, _color.B));
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      base.Load(sceneDataNode);
      var direction = sceneDataNode.ReadVectorParameter("direction");
      Init(direction.Normalized());
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