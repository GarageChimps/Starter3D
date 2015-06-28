using System.Collections.Generic;

namespace ThreeAPI.scene.geometry.factories
{
  public interface IFaceFactory
  {
    IFace CreateFace(List<int> indices);
  }
}