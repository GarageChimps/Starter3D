using System;
using OpenTK;

namespace Starter3D.API.geometry.primitives
{
  public class Torus : TesselatedMesh
  {
    private float _r1;

    public Torus(int numU, int numV)
      : base(numU, numV, 0, (float)Math.PI * 2.0f, 0, (float)Math.PI * 2.0f)
    {
      _r1 = 0.5f;
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
      var position = new Vector3((float)((_r1 + Math.Cos(v)) * Math.Cos(u)), (float)(Math.Sin(v)), (float)((_r1 + Math.Cos(v)) * Math.Sin(u)));
      var tx = (float)(-Math.Sin(u));
      var tz = (float)(Math.Cos(u));

      var normal = position.Normalized();
      return new Vertex(position, normal, new Vector2(u / ((float)Math.PI * 2.0f), v / ((float)Math.PI * 2.0f)));
    }
  }
}