using System.Collections.Generic;

namespace ThreeAPI.scene
{
  public interface IFaceFactory
  {
    IFace CreateFace(List<int> indices);
  }
}