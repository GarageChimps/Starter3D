using System.Collections.Generic;
using OpenTK;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  /// <summary>
  /// Interface for a generic element of a scene
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface ISceneNode : IRenderElement
  {
    IEnumerable<ISceneNode> Children { get; }   //An element can have zero, one or N children elements
    ISceneNode Parent { get; set; }             //Parent node for this element
    
    void AddChild(ISceneNode child);
    void RemoveChild(ISceneNode child);
    void Load(ISceneDataNode sceneDataNode);
    void Save(ISceneDataNode sceneDataNode);
    
    /// <summary>
    /// Returns all nodes of type provided that are part of this node subtree
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>List of nodes</returns>
    IEnumerable<T> GetNodes<T>() where T : class, ISceneNode;
  }
}