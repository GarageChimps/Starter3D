using System.Globalization;
using OpenTK;
using OpenTK.Graphics;
using Starter3D.API.renderer;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class PointLight : LightNode
  {
    
    public PointLight(Color4 color)
      : base(color)
    {
    }

    public PointLight()
    {
      
    }
    
    public override void ConfigureRenderer(IRenderer renderer)
    {
      var transform = ComposeTransform();
      renderer.AddVectorParameter("lightPosition", transform.Row3.Xyz);
      renderer.AddVectorParameter("lightColor", new Vector3(_color.R, _color.G, _color.B));
      base.ConfigureRenderer(renderer);
    }
  }
}