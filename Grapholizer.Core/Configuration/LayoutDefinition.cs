using System.Xml.Serialization;


namespace Grapholizer.Core.Configuration
{
  public class LayoutDefinition
  {
    [XmlAttribute]
    public string Style { get; set; }

    [XmlAttribute]
    public string Modifier { get; set; }
  }
}
