using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grapholizer.Core.Layouts
{
  public class TreeLayout
  {
    public void Layout(Dictionary<string, Node> nodes, Node root)
    {
      HashSet<string> visited = new HashSet<string>();
      MeasureRecursively(nodes, root, visited);

      visited.Clear();
      LayoutRecursively(nodes, root, 0, 100, visited, 1);
    }


    private int MeasureRecursively(Dictionary<string, Node> nodes, Node current, HashSet<string> visited)
    {
      string key = current.Type + "-" + current.Id;
      if (visited.Contains(key))
        return 0;
      visited.Add(key);

      int width = 0;
      for (int i = 0; i < current.Edges.Length; ++i)
      {
        Edge e = current.Edges[i];
        string nextKey = e.TargetNodeType + "-" + e.TargetNodeId;
        Node next = nodes[nextKey];

        width += MeasureRecursively(nodes, next, visited);
      }

      if (width == 0)
        width = 1;

      // Calculated width of children stored temporarily as the X-coordinate
      current.X = width;

      return width;
    }


    private void LayoutRecursively(Dictionary<string,Node> nodes, Node current, int min, int max, HashSet<string> visited, int level)
    {
      string key = current.Type + "-" + current.Id;
      if (visited.Contains(key))
        return;
      visited.Add(key);

      // Calculated width of children
      int width = current.X;

      int x = (max - min) / 2 + min;
      int y = level * 50;
      current.X = y;
      current.Y = x;

      if (current.Edges.Length > 0)
      {
        int stepSize = (max - min) / width;
        int stepStart = min;

        for (int i = 0; i < current.Edges.Length; ++i)
        {
          Edge e = current.Edges[i];
          string nextKey = e.TargetNodeType + "-" + e.TargetNodeId;

          if (!visited.Contains(nextKey))
          {
            Node next = nodes[nextKey];

            int nextMin = stepStart;
            int nextMax = nextMin + next.X * stepSize;

            LayoutRecursively(nodes, next, nextMin, nextMax, visited, level + 1);

            stepStart = nextMax;
          }
        }
      }
    }
  }
}
