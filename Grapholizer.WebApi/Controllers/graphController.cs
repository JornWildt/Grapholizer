using System;
using System.Linq;
using System.Web.Http;
using Grapholizer.Core;
using Grapholizer.WebApi.Models.JSON;


namespace Grapholizer.WebApi.Controllers
{
  public class graphController : ApiController
  {
    #region Dependencies

    public GraphService GraphService { get; set; }

    #endregion


    public object Get(string name, string node, string id, int size = 5)
    {
      Graph g = GraphService.GetGraph(name, node, id, size);

      Random r = new Random();

      GraphJS gjs = new GraphJS
      {
        nodes = g.Nodes.Select(n => new NodeJS
        {
          id = n.Id,
          label = n.Label,
          x = n.X,
          y = n.Y,
          size = n.Size ?? 1,
          color = n.Color,
          type = n.Symbol,
          selfLink = string.Format("http://localhost/grapholizer.api/graph/example1/{0}/{1}?size=5", n.Type, n.Id)
        }).ToArray(),
        edges = g.Nodes.SelectMany(n => n.Edges.Select(e => new EdgeJS
        {
          id = Guid.NewGuid().ToString(),
          source = n.Id,
          target = e.TargetNodeId,
          label = e.Label,
          size = e.Size ?? 1,
          color = e.Color,
          type = e.Symbol
        })).ToArray()
      };

      return gjs;
    }
  }
}
