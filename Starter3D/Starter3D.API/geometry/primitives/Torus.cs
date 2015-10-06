using System;
using OpenTK;

namespace Starter3D.API.geometry.primitives
{
  public class Torus : TesselatedMesh
  {
    public Torus(int numU, int numV)
      : base(numU, numV, 0, (float)Math.PI * 2.0f, 0, (float)Math.PI * 2.0f)
    {
    }
    //vx = (r1 + np.cos(v))*np.cos(u)
    //    vy = (r1 + np.cos(v))*np.sin(u)
    //    vz = np.sin(v)
    //    tx = -np.sin(u);
    //    ty = np.cos(u);
    //    sx = -np.cos(u)*np.sin(v);
    //    sy = -np.sin(u)*np.sin(v);
    //    sz = np.cos(v);
    //    nx = ty*sz
    //    ny = -tx*sz;
    //    nz = tx*sy - ty*sx;
    protected override IVertex GetVertex(float u, float v)
    {
      var position = new Vector3((float)(Math.Sin(u) * Math.Cos(v)), (float)(Math.Sin(u) * Math.Sin(v)),
        (float)(Math.Cos(u)));
      var normal = position.Normalized();
      return new Vertex(position, normal, new Vector2(u, v));
    }
  }
}