using System;
using OpenTK;

namespace Starter3D.Plugin.Physics
{
  public class Asteroid
  {
    private float _radius;
    private float _speed;
    private float _size;
    private Vector3 _center;

    public Asteroid(float radius, float speed, Vector3 center, float size)
    {
      _radius = radius;
      _speed = speed;
      _center = center;
      _size = size;
    }

    public Matrix4 GetTransformantion(float angle)
    {
      var scale = Matrix4.CreateScale(_size);
      var translation = Matrix4.CreateTranslation(GetPosition(angle));
      return scale*translation;
    }

    private Vector3 GetPosition(float angle)
    {
      var x = (float)(_center.X + _radius * Math.Cos(_speed * angle));
      var y = (float)(_center.Y + _radius * Math.Sin(_speed * angle));
      return new Vector3(x, 0, y);
    }
  }
}