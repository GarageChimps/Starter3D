namespace Starter3D.API.geometry.loaders
{
  /// <summary>
  /// A mesh loader is a module that loads a mesh from file and stores its data in an IMesh object
  /// @author Alejandro Echeverria
  /// @data July-2015
  /// </summary>
  public interface IMeshLoader
  {
    void Load(IMesh mesh, string filePath);
  }
}