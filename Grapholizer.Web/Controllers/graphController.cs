using System.Web.Mvc;
using Grapholizer.Web.Models;


namespace Grapholizer.Web.Controllers
{
  public class graphController : Controller
  {
    // GET: graph
    public ActionResult Index(string name, string node, string id, int size = 4)
    {
      GraphDisplayModel model = new GraphDisplayModel
      {
        Name = name,
        Node = node,
        Id = id,
        Size = size
      };

      return View(model);
    }
  }
}
