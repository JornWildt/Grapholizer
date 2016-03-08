using System.Web.Mvc;
using Grapholizer.Web.Models;


namespace Grapholizer.Web.Controllers
{
  public class graphController : Controller
  {
    // GET: graph
    public ActionResult Index(string name, string node, string id)
    {
      GraphDisplayModel model = new GraphDisplayModel
      {
        Name = name,
        Node = node,
        Id = id
      };

      return View(model);
    }
  }
}
