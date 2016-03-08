using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Grapholizer.Core;
using Grapholizer.Core.DataAccess;
using Grapholizer.WebApi.Models.JSON;
using Grapholizer.WebApi.Utility;

namespace Grapholizer.WebApi.Controllers
{
  public class graphController : ApiController
  {
    #region Dependencies

    public IDataProvider DataProvider { get; set; }

    #endregion


    // GET: api/graph/5
    public object Get(string name, string node, string id)
    {
      using (IUnitOfWorkManager<SqlClientUnitOfWork> uow = new WebUnitOfWorkManager())
      {
        // FIXME: improve this!
        ((SqlClientDataProvider)DataProvider).UnitOfWorkManager = uow;

        GraphService gs = new GraphService();
        gs.DataProvider = DataProvider;

        GraphSegment g = gs.GetGraphSegment(name, node, id);

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
}
