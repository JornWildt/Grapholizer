using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grapholizer.Core.Configuration
{
  public class EdgeDefinition
  {
    [XmlAttribute]
    public string TargetNode { get; set; }

    public string SQL { get; set; }
  }
}
