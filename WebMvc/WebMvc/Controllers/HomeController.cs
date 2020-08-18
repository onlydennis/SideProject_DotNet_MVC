using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            KeyValuesDAO dao = new KeyValuesDAO();

            try
            {
                var tmp = dao.Test();
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return View();
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