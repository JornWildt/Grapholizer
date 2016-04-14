using System;
using System.Web.Mvc;
using Grapholizer.Web.Models;
using Ramone;

namespace Grapholizer.Web.Controllers
{
  public class graphController : Controller
  {
    // GET: graph
    public ActionResult Index(string name, string node, string id, int size = 4)
    {
      ISession session = RamoneConfiguration.NewSession(new Uri(AppSettings.ApiBaseUrl.Value));
      Request request = session.Bind("meta/{name}", new { name = name }).AcceptJson();


      using (var response = request.Get<dynamic>())
      {
        string title = response.Body.title;

        GraphDisplayModel model = new GraphDisplayModel
        {
          ApiBaseUrl = AppSettings.ApiBaseUrl.Value,
          Name = name,
          Node = node,
          Id = id,
          Size = size,
          Title = title
        };

        return View(model);
      }
    }
  }
}
