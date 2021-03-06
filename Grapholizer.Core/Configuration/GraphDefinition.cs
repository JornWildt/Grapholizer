﻿using System.Xml.Serialization;


namespace Grapholizer.Core.Configuration
{
  [XmlRoot("Graph", Namespace = Constants.Namespace)]
  public class GraphDefinition
  {
    [XmlAttribute]
    public string Title { get; set; }

    public LayoutDefinition Layout { get; set; }

    [XmlElement("Node")]
    public NodeDefinition[] Nodes { get; set; }
  }
}
