using System.Web.Http;
using Grapholizer.Core;
using Grapholizer.WebApi.Models.JSON;

namespace Grapholizer.WebApi.Controllers
{
  public class metaController : ApiController
  {
    #region Dependencies

    public GraphService GraphService { get; set; }

    #endregion


    public object Get(string name)
    {
      GraphMeta meta = GraphService.GetGraphMeta(name);
      return new MetaJS
      {
        title = meta.Title
      };
    }
  }
}