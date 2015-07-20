using System.Collections.Generic;

namespace Starter3D
{
  public interface IFaceFactory
  {
    IFace CreateFace(List<int> indices);
  }
}