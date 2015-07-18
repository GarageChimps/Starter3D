using OpenTK;
using OpenTK.Graphics;
using Starter3D.API.renderer;

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

    public Vector3 GetPosition()
    {
      var transform = ComposeTransform();
      return transform.Row3.Xyz;
    }
    
    public override void Configure(IRenderer renderer)
    {
      renderer.AddVectorParameter("lightPosition", GetPosition());
      renderer.AddVectorParameter("lightColor", new Vector3(_color.R, _color.G, _color.B));
    }

    public override void Update(IRenderer renderer)
    {
      //Configure(renderer);
    }
  }
}