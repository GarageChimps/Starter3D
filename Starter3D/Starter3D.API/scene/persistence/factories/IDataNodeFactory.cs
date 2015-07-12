using System.Xml.Linq;

namespace Starter3D.API.scene.persistence.factories
{
  public interface IDataNodeFactory
  {
    IDataNode CreateXmlDataNode(XElement element);
  }
}