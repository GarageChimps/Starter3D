using System;

namespace Starter3D
{
  public class Mesh: BaseSceneNode, IMesh
  {
    protected IGeometry _geometry;
    public Mesh (IGeometry geometry)
    {
      _geometry = geometry;
    }

    public IGeometry Geometry {
      get { return _geometry; }
    }
  }
}

