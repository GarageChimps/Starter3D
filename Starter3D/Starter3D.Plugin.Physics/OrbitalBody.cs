using System;
using System.ComponentModel;
using System.Windows;
using OpenTK;
using Starter3D.API.physics;

namespace Starter3D.Plugin.Physics
{
  public class OrbitalBody : IRigidSolid
  {
    private float _mass;
    private float _size;
    private Vector3 _position;
    private Vector3 _linearMomentum;

    public float Mass
    {
      get { return _mass; }
    }

    public Vector3 Position
    {
      get { return _position; }
      set { _position = value; }
    }

    public Vector3 LinearMomentum
    {
      get { return _linearMomentum; }
      set { _linearMomentum = value; }
    }

    public Vector3 Velocity
    {
      get { return LinearMomentum / Mass; }
    }

    public Quaternion Rotation
    {
      get { throw new NotImplementedException(); }
    }

    public Vector3 AngularMomentum
    {
      get { throw new NotImplementedException(); }
    }

    public OrbitalBody(Vector3 initialPosition, Vector3 initialMomentum, float mass, float size)
    {
      _size = size;
      _mass = mass;
      _position = initialPosition;
      _linearMomentum = initialMomentum;
      
    }

    public Matrix4 GetTransformantion(float angle)
    {
      var scale = Matrix4.CreateScale(_size);
      var translation = Matrix4.CreateTranslation(_position);
      return scale * translation;
    }

    //private Vector3 GetPosition(float angle)
    //{
    //  var x = (float)(_center.X + _radius * Math.Cos(_speed * angle));
    //  var y = (float)(_center.Y + _radius * Math.Sin(_speed * angle));
    //  return new Vector3(x, 0, y);
    //}


  }
}