using System;
using Starter3D.API.renderer;

namespace Starter3D.Renderers
{
  public class RendererFactory : IRendererFactory
  {
    public IRenderer CreateRenderer(RendererType type)
    {
      switch (type)
      {
        case RendererType.OpenGL:
          return new OpenGLRenderer();
        case RendererType.Direct3D:
          return new Direct3DRenderer();
        case RendererType.Composite:
          return new CompositeRenderer();
        default:
          throw new ArgumentOutOfRangeException("type");
      }
    }
  }
}