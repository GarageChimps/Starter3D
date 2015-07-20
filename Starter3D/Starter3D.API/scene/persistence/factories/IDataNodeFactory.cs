using System.Xml.Linq;

namespace Starter3D
{
  public interface IDataNodeFactory
  {
    IDataNode CreateXmlDataNode(XElement element);
  }
}