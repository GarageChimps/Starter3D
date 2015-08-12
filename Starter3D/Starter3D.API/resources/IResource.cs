using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  /// <summary>
  /// A resource used in the application, that can be loaded from file and configured
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IResource
  {
    /// <summary>
    /// Prepares the resource for rendering, should be called at the beginning of the rendering process
    /// </summary>
    /// <param name="renderer"></param>
    void Configure(IRenderer renderer);

    /// <summary>
    /// Sets the render to use this resource for the current rendering frame
    /// </summary>
    /// <param name="renderer"></param>
    void Render(IRenderer renderer);

    /// <summary>
    /// Loads ther esource from file or other data source
    /// </summary>
    /// <param name="dataNode">Data source</param>
    /// <param name="resourceManager">Manager of resources</param>
    void Load(IDataNode dataNode, IResourceManager resourceManager);
  }
}