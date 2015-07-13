using System.Xml.Linq;

namespace Starter3D.API.scene.persistence.factories
{
  public interface ISceneDataNodeFactory
  {
    ISceneDataNode CreateXmlDataNode(XElement element);
  }
}