using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.physics
{
  public class PhysicsEngine : IPhysicsEngine
  {
    private readonly ISolver _solver;
    private readonly List<IRigidSolid> _objects = new List<IRigidSolid>();
    private readonly List<IForce> _forces = new List<IForce>(); 

    public PhysicsEngine(ISolver solver)
    {
      _solver = solver;
    }

    public void AddObject(IRigidSolid obj)
    {
      _objects.Add(obj);
    }

    public void AddForce(IForce force)
    {
      _forces.Add(force);
    }

    public void Update(float timeStep)
    {
      var forceMap = CalculateForces();
      _solver.Update(_objects, forceMap, timeStep);
    }

    private IDictionary<IRigidSolid, Vector3> CalculateForces()
    {
      var forceDictionary = new Dictionary<IRigidSolid, Vector3>();
      foreach (var obj in _objects)
      {
        forceDictionary.Add(obj, new Vector3());
      } 

      foreach (var force in _forces)
      {
        var currentForceDictionary = force.CalculateForce(_objects);
        foreach (var objForcePair in currentForceDictionary)
        {
          forceDictionary[objForcePair.Key] += objForcePair.Value;
        }
      }
      return forceDictionary;

    }

   
  }
}