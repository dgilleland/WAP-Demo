using OffTheCuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OffTheCuff.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var controller = BackEndController.Instance;
            var data = controller.GetCourseInfo(5);
            return View(data);
        }
        [HttpPost]
        public ActionResult MakeWork([Bind(Prefix = "work")] Assignment work)
        {
            // TODO: Fix this bug, and figure out how the model binding works again
            var controller = BackEndController.Instance;
            var data = controller.GetCourseInfo(5);
            //var work = new Assignment { Name = name, Weight = weight };
            data.Assignments.Add(work);
            return View("Index", data);
        }
        [HttpPost]
        public ActionResult Index(Student me)
        {
            var controller = BackEndController.Instance;
            var data = controller.GetCourseInfo(5);
            data.Students.Add(me);
            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}