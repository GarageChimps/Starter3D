using System;
using OpenTK;

namespace Starter3D.API.geometry.primitives
{
  public class Sphere : TesselatedMesh
  {
    public Sphere(int numU, int numV)
      : base(numU, numV, 0, (float)Math.PI, 0, (float)Math.PI * 2.0f)
    {
    }

    protected override IVertex GetVertex(float u, float v)
    {
      var position = new Vector3((float) (Math.Sin(u)*Math.Cos(v)), (float) (Math.Cos(u)), (float) (Math.Sin(u)*Math.Sin(v)));
      var normal = position.Normalized();
      return new Vertex(position, normal, new Vector2(1 - v / ((float)Math.PI * 2.0f), u / (float)Math.PI));
    }
  }
}