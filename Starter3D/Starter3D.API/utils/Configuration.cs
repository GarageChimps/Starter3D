using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Starter3D.API.utils
{
  public class Configuration : IConfiguration
  {
    private const string ConfigurationPath = "config.xml";
    private string _scenePath;
    private string _resourcesPath;

    public string ScenePath
    {
      get { return _scenePath; }
    }

    public string ResourcesPath
    {
      get { return _resourcesPath; }
    }

    public Configuration()
    {
      Load();
    }

    private void Load()
    {
      if (!File.Exists(ConfigurationPath))
        return;

      var xmlDoc = XDocument.Load(ConfigurationPath);
      var xmlConfig = xmlDoc.Elements("Configuration").First();

      _scenePath = xmlConfig.Attribute("scene").Value;
      _resourcesPath = xmlConfig.Attribute("resources").Value;
    }
  }
}