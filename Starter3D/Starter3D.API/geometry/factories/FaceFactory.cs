using System;
using System.Collections.Generic;

namespace ThreeAPI.geometry.factories
{
  public class FaceFactory : IFaceFactory
  {
    public IFace CreateFace(List<int> indices )
    {
      if (indices.Count == 3)
        return new Face(indices[0], indices[1], indices[2]);
      if (indices.Count == 4)
        return new Face(indices[0], indices[1], indices[2], indices[3]);
      throw new ApplicationException("Invalid number of indices in face");
    }

  }
}