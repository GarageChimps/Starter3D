using Starter3D.API.renderer;

namespace Starter3D.Renderers
{
  public interface IRendererFactory
  {
    IRenderer CreateRenderer(RendererType type);
  }
}