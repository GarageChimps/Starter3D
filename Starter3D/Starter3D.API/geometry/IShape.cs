using OpenTK;
using Starter3D.API.renderer;
using Starter3D.API.resources;

namespace Starter3D.API.geometry
{
  /// <summary>
  /// Common interface for all geometrical shapes
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IShape
  {
    string Name { get; }               //Name of this shape
    IMaterial Material { get; set; }   //Material associated to this shape
    
    /// <summary>
    /// Loads the shape from file, using the suitable loader
    /// </summary>
    /// <param name="filePath">File path</param>
    void Load(string filePath);        
    
    /// <summary>
    /// Saves a shape into file, using the suitable saver
    /// </summary>
    /// <param name="filePath">File path</param>
    void Save(string filePath);

    /// <summary>
    /// Configures this shape for rendering
    /// </summary>
    /// <param name="renderer">Renderer</param>
    void Configure(IRenderer renderer);

    /// <summary>
    /// Renders this shape
    /// </summary>
    /// <param name="renderer">Renderer that will perform the rendering</param>
    /// <param name="modelTransform">Model to world transformation for this shape</param>
    void Render(IRenderer renderer, Matrix4 modelTransform);
  }
}