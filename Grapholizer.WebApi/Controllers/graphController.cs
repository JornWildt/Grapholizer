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
      GraphSegment g = GraphService.GetGraphSegment(name, node, id, size);

      Random r = new Random();

      GraphJS gjs = new GraphJS
      {
        nodes = g.Nodes.Select(n => new NodeJS
        {
          id = n.Id,
          label = n.Label,
          x = r.Next(100),
          y = r.Next(100),
          size = n.Size ?? 1,
          color = n.Color,
          type = n.Type
        }).ToArray(),
        edges = g.Nodes.SelectMany(n => n.Edges.Select(e => new EdgeJS
        {
          id = Guid.NewGuid().ToString(),
          source = n.Id,
          target = e.TargetNodeId,
          size = e.Size ?? 1,
          color = e.Color,
          type = e.Type
        })).ToArray()
      };

      return gjs;
    }
  }
}
