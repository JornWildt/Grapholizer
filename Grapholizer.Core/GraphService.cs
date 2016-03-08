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


    public GraphSegment GetGraphSegment(string graphName, string nodeType, string id)
    {
      GraphDefinition graph = GraphParser.GetGraphDefinition(graphName);
      Node node = GetNode(graph, nodeType, id);

      Dictionary<string, Node> nodes = new Dictionary<string, Node>();
      nodes[node.Id] = node;

      foreach (Edge edge in node.Edges)
      {
        if (!nodes.ContainsKey(edge.TargetNode))
        {
          nodes[edge.TargetNode] = new Node
          {
            Id = edge.TargetNode,
            Label = edge.Label,
            Edges = new Edge[0]
          };
        }
      }

      return new GraphSegment
      {
        Nodes = nodes.Select(n => n.Value).ToArray()
      };
    }


    protected Node GetNode(GraphDefinition graph, string nodeType, string id)
    {
      NodeDefinition nodeDef = GetNodeDefinition(graph, nodeType);

      DataRow node = ReadNode(nodeDef, id);
      Edge[] edges = nodeDef.Edges.SelectMany(e => ReadEdges(e, id)).ToArray();

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
          TargetNode = row["_Id"].ToString()
        });
    }
  }
}
