using System.Collections.Generic;

namespace Starter3D.API.geometry
{
  public class CircleCollection : Curve
  {
    private readonly List<Circle> _circles = new List<Circle>();

    public CircleCollection(string name) : base(name)
    {
      
    }

    public void AddCircle(Circle circle)
    {
      _circles.Add(circle);
      foreach (var point in circle.Points)
      {
        AddPoint(point);
      }
    }
  }
}