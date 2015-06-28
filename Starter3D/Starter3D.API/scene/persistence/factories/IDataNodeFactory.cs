using System.Xml.Linq;

namespace ThreeAPI.scene.persistence.factories
{
  public interface IDataNodeFactory
  {
    IDataNode CreateXmlDataNode(XElement element);
  }
}