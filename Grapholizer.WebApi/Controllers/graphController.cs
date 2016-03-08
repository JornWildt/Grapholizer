﻿using System;
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


    public object Get(string name, string node, string id)
    {
      GraphSegment g = GraphService.GetGraphSegment(name, node, id);

      Random r = new Random();

      GraphJS gjs = new GraphJS
      {
        nodes = g.Nodes.Select(n => new NodeJS
        { id = n.Id, label = n.Label, x = r.Next(10), y = r.Next(10), size = n.Edges.Length + 1 }).ToArray(),
        edges = g.Nodes.SelectMany(n => n.Edges.Select(e => new EdgeJS { id = Guid.NewGuid().ToString(), source = n.Id, target = e.TargetNode })).ToArray()
      };

      return gjs;
    }
  }
}
