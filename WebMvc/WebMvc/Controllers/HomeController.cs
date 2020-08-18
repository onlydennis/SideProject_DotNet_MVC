using DAL;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModel;

namespace WebMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReadData(KeyValuesViewModel query)
        {
            HomeService service = new HomeService();

            List<KeyValuesViewModel> data = new List<KeyValuesViewModel>();

            try
            {
                data = service.ReadData(query);

                var obj = new
                {
                    data = data,
                    total = data.Count
                };

                var result = JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                string exMsg = ex.Message;
            }

            return Json(new { data = "", total = 0 });
        }
    }
}