using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grapholizer.Web.Controllers
{
  public class graphController : Controller
  {
    // GET: graph
    public ActionResult Index()
    {
      return View();
    }

    // GET: graph/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: graph/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: graph/Create
    [HttpPost]
    public ActionResult Create(FormCollection collection)
    {
        try
        {
            // TODO: Add insert logic here

            return RedirectToAction("Index");
        }
        catch
        {
            return View();
        }
    }

    // GET: graph/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: graph/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
        try
        {
            // TODO: Add update logic here

            return RedirectToAction("Index");
        }
        catch
        {
            return View();
        }
    }

    // GET: graph/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: graph/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
        try
        {
            // TODO: Add delete logic here

            return RedirectToAction("Index");
        }
        catch
        {
            return View();
        }
    }
  }
}
