using Starter3D.API.renderer;

namespace Starter3D.API.scene.nodes
{
  public interface IRenderElement
  {
    void Configure(IRenderer renderer);
    void Update(IRenderer renderer);
    void Render(IRenderer renderer); 
  }
}