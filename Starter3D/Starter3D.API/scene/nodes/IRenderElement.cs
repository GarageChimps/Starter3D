using Starter3D.API.renderer;

namespace Starter3D.API.scene.nodes
{
  /// <summary>
  /// Interface for a generic renderable element
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IRenderElement
  {
    /// <summary>
    /// Prepares the renderer to work with this element in the future
    /// </summary>
    /// <param name="renderer"></param>
    void Configure(IRenderer renderer);

    /// <summary>
    /// Specifies to the render that this element will be used or renderer in the current frame
    /// </summary>
    /// <param name="renderer"></param>
    void Render(IRenderer renderer); 
  }
}