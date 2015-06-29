using System.Collections.Generic;

namespace ThreeAPI.geometry.factories
{
  public interface IFaceFactory
  {
    IFace CreateFace(List<int> indices);
  }
}