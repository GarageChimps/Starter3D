﻿using System;
using OpenTK;

namespace Starter3D
{
  public class BoxGeometry: Geometry
  {
    public BoxGeometry (float width, float height, float depth, 
      int widthSegments, int heightSegments, int depthSegments)
    {
      Vector3 zero = new Vector3 (-0.5, -0.5, -0.5);

      _vertices.Add (new Vector3 (0.0, 0.0, 0.0) - zero); //0
      _vertices.Add (new Vector3 (0.0, 0.0, 1.0) - zero); //1
      _vertices.Add (new Vector3 (0.0, 1.0, 0.0) - zero); //2
      _vertices.Add (new Vector3 (0.0, 1.0, 1.0) - zero); //3
      _vertices.Add (new Vector3 (1.0, 0.0, 0.0) - zero); //4
      _vertices.Add (new Vector3 (1.0, 0.0, 1.0) - zero); //5
      _vertices.Add (new Vector3 (1.0, 1.0, 0.0) - zero); //6
      _vertices.Add (new Vector3 (1.0, 1.0, 1.0) - zero); //7

      _faces.Add (new Face (0, 6, 4));
      _faces.Add (new Face (0, 2, 6)); 
      _faces.Add (new Face (0, 3, 2)); 
      _faces.Add (new Face (0, 1, 3)); 
      _faces.Add (new Face (2, 7, 6)); 
      _faces.Add (new Face (2, 3, 7)); 
      _faces.Add (new Face (4, 6, 7)); 
      _faces.Add (new Face (4, 7, 5)); 
      _faces.Add (new Face (0, 4, 5)); 
      _faces.Add (new Face (0, 5, 1)); 
      _faces.Add (new Face (1, 5, 7)); 
      _faces.Add (new Face (1, 7, 3));

    }
  }
}

