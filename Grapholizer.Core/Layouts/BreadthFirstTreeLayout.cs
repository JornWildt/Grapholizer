using System.Collections.Generic;
using System.Linq;


namespace Grapholizer.Core.Layouts
{
  public class BreadthFirstTreeLayout : ILayout
  {
    class BFNode
    {
      public Node N { get; set; }
      public int Rank { get; set; }

      public BFNode(Node n)
      {
        N = n;
      }
    }

    public void Layout(Dictionary<string, Node> nodes, Node root)
    {
      Dictionary<string, BFNode> bfnodes = nodes.ToDictionary(n => n.Key, n => new BFNode(n.Value));
      BFNode bfroot = bfnodes[root.Type + "-" + root.Id];

      Layout(bfnodes, bfroot);
    }


    private void Layout(Dictionary<string, BFNode> nodes, BFNode root)
    {
      CalculateRank(nodes, root);

      HashSet<string> visited = new HashSet<string>();
      MeasureRecursively(nodes, root, visited);

      visited.Clear();
      LayoutRecursively(nodes, root, 0, 100, visited, 1);
    }


    private void CalculateRank(Dictionary<string, BFNode> nodes, BFNode root)
    {
      root.Rank = 1;

      Queue<BFNode> workset = new Queue<BFNode>();
      workset.Enqueue(root);

      while (workset.Count > 0)
      {
        BFNode current = workset.Dequeue();

        for (int i = 0; i < current.N.Edges.Length; ++i)
        {
          Edge e = current.N.Edges[i];
          string nextKey = e.TargetNodeType + "-" + e.TargetNodeId;
          BFNode next = nodes[nextKey];

          if (next.Rank == 0)
          {
            next.Rank = current.Rank + 1;
            workset.Enqueue(next);
          }
        }
      }
    }


    private int MeasureRecursively(Dictionary<string, BFNode> nodes, BFNode current, HashSet<string> visited)
    {
      string key = current.N.Type + "-" + current.N.Id;
      if (visited.Contains(key))
        return 0;
      visited.Add(key);

      // Foreach edge, calculate the width of the underlying tree - the width of this node is then the sum of all sub-tree-widths

      int width = 0;
      for (int i = 0; i < current.N.Edges.Length; ++i)
      {
        Edge e = current.N.Edges[i];
        string nextKey = e.TargetNodeType + "-" + e.TargetNodeId;
        BFNode next = nodes[nextKey];

        // Only move forwards in the graph
        if (next.Rank > current.Rank)
          width += MeasureRecursively(nodes, next, visited);
      }

      if (width == 0)
        width = 1;

      // Calculated width of children stored temporarily as the X-coordinate
      current.N.X = width;

      return width;
    }


    private void LayoutRecursively(Dictionary<string, BFNode> nodes, BFNode current, int min, int max, HashSet<string> visited, int level)
    {
      string key = current.N.Type + "-" + current.N.Id;
      if (visited.Contains(key))
        return;
      visited.Add(key);

      // Calculated width of children
      int width = current.N.X;

      int x = (max - min) / 2 + min;
      int y = level * 50;
      current.N.X = y;
      current.N.Y = x;

      if (current.N.Edges.Length > 0)
      {
        int stepSize = (max - min) / width;
        int stepStart = min;

        for (int i = 0; i < current.N.Edges.Length; ++i)
        {
          Edge e = current.N.Edges[i];
          string nextKey = e.TargetNodeType + "-" + e.TargetNodeId;

          BFNode next = nodes[nextKey];

          // Only move forwards in the graph
          if (!visited.Contains(nextKey) && next.Rank > current.Rank)
          {
            int nextMin = stepStart;
            int nextMax = nextMin + next.N.X * stepSize;

            LayoutRecursively(nodes, next, nextMin, nextMax, visited, level + 1);

            stepStart = nextMax;
          }
        }
      }
    }
  }
}
