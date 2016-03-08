using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Grapholizer.Core.Configuration;
using Grapholizer.Core.DataAccess;


namespace Grapholizer.Core
{
  public class GraphService
  {
    #region Dependencies

    public IDataProvider DataProvider { get; set; }

    #endregion


    public GraphSegment GetGraphSegment(string graphName, string nodeType, string id, int size)
    {
      if (size < 1)
        size = 1;
      if (size > 10)
        size = 10;

      GraphDefinition graph = GraphParser.GetGraphDefinition(graphName);
      Dictionary<string, Node> nodes = new Dictionary<string, Node>();

      Node root = GetNodeRecursively(graph, nodeType, id, nodes, size);

      return new GraphSegment
      {
        Nodes = nodes.Select(n => n.Value).ToArray()
      };
    }


    protected Node GetNodeRecursively(GraphDefinition graph, string nodeType, string id, Dictionary<string, Node> nodes, int level)
    {
      if (level <= 0)
        return null;

      string nodeKey = nodeType + "-" + id;
      if (nodes.ContainsKey(nodeKey))
        return null;

      Node node = GetNode(graph, nodeType, id, level > 1);
      nodes[nodeKey] = node;

      foreach (Edge e in node.Edges)
      {
        GetNodeRecursively(graph, e.TargetNodeType, e.TargetNodeId, nodes, level - 1);
      }

      return node;
    }


    protected Node GetNode(GraphDefinition graph, string nodeType, string id, bool getEdges)
    {
      NodeDefinition nodeDef = GetNodeDefinition(graph, nodeType);

      DataRow node = ReadNode(nodeDef, id);
      Edge[] edges = getEdges
        ? nodeDef.Edges.SelectMany(e => ReadEdges(e, id)).ToArray()
        : new Edge[0];

      return new Node
      {
        Id = node["_Id"].ToString(),
        
        Label = node["_Label"].ToString(),
        Edges = edges
      };
    }

    protected NodeDefinition GetNodeDefinition(GraphDefinition graph, string nodeType)
    {
      NodeDefinition ndef = graph.Nodes.FirstOrDefault(n => n.Key == nodeType);
      if (ndef == null)
        throw new InvalidOperationException(string.Format("Unknown node type '{0}' in graph '{1}'.", nodeType, graph.Title));
      return ndef;
    }


    protected DataRow ReadNode(NodeDefinition nodeDef, string id)
    {
      string sql = nodeDef.SQL;
      DataRow row = DataProvider.ReadSingle(sql, new { Id = id });
      return row;
    }


    private IEnumerable<Edge> ReadEdges(EdgeDefinition edgeDef, string id)
    {
      string sql = edgeDef.SQL;
      DataTable edges = DataProvider.Read(sql, new { Id = id });
      return edges.Rows.Cast<DataRow>()
        .Where(row => row["_Id"] != DBNull.Value)
        .Select(row => new Edge
        {
          Label = row["_Label"].ToString(),
          TargetNodeType = edgeDef.TargetNode,
          TargetNodeId = row["_Id"].ToString()
        });
    }
  }
}
