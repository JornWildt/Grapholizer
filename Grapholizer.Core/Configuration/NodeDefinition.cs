using System.Xml.Serialization;

namespace Grapholizer.Core.Configuration
{
  public class NodeDefinition
  {
    [XmlAttribute]
    public string Label { get; set; }

    [XmlAttribute]
    public string Key { get; set; }

    public string SQL { get; set; }

    [XmlElement("Edge")]
    public EdgeDefinition[] Edges { get; set; }
  }
}
