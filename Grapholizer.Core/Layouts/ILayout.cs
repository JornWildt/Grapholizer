using System.Collections.Generic;


namespace Grapholizer.Core.Layouts
{
  public interface ILayout
  {
    void Layout(Dictionary<string, Node> nodes, Node root);
  }
}
