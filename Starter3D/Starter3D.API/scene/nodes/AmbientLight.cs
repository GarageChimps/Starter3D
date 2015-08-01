using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.scene.nodes
{
  public class AmbientLight : LightNode
  {
    public override void Configure(IRenderer renderer)
    {
      renderer.SetVectorParameter("ambientLight", new Vector3(_color.R, _color.G, _color.B));
    }

    public override void Render(IRenderer renderer)
    {
      Configure(renderer);
    }
  }
}