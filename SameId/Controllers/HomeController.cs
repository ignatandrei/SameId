using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SameId.Models;
using SameIdAlert;


namespace SameId.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "SameID user application - to find if the users are on the same page and editing same objects";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "SameID user application - to find if the users are on the same page and editing same objects";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Andrei Ignat";

            return View();
        }

        [Authorize()]
        public ActionResult Edit(int id)
        {
            var prod=Product.GetFromId(id);
            return View(prod);
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Edit(Product prod)
        {
            return View(prod);
        }

        [SameIdSkipAttribute]
        [Authorize()]
        public ActionResult View(int id)
        {
            var prod = Product.GetFromId(id);
            return View(prod);
        }

    }
}
