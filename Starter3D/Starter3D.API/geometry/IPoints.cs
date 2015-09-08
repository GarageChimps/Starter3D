using OpenTK;

namespace Starter3D.API.geometry
{
  /// <summary>
  /// Interface for point cloud geometry, not implemented yet
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IPoints : IShape
  {
    void AddPoint(Vector3 position);
    void Clear();
  }
}