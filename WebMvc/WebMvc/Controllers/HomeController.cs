using DAL;
using Newtonsoft.Json;
using Service;
using Shared.Core.Data;
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

            data = service.ReadData(query);

            var obj = new
            {
                data = data,
                total = data.Count
            };

            var result = JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
            return Content(result, "application/json");
        }

        public ActionResult Edit(string keyValueName, string text)
        {
            HomeService service = new HomeService();

            List<KeyValuesViewModel> data = new List<KeyValuesViewModel>();

            KeyValuesViewModel query = new KeyValuesViewModel();

            query.KeyValueName = keyValueName;
            query.Text = text;

            data = service.ReadData(query);

            return Json(new ApplicationMessage { IsOk = true, Data = data.FirstOrDefault() });
        }

        public ActionResult Update(KeyValuesViewModel data)
        {
            HomeService service = new HomeService();

            bool res = service.Update(data, out string errMsg);

            if (res)
            {
                var result = JsonConvert.SerializeObject(new { IsOk = true, Message = "儲存成功" }, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
                return Content(result, "application/json");
            }
            else
            {
                var result = JsonConvert.SerializeObject(new { IsOk = false, Message = "儲存失敗" + errMsg }, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
                return Content(result, "application/json");
            }
        }

        public ActionResult Create(KeyValuesViewModel data)
        {
            HomeService service = new HomeService();

            bool res = service.Create(data, out string errMsg);

            if (res)
            {
                var result = JsonConvert.SerializeObject(new { IsOk = true, Message = "儲存成功" }, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
                return Content(result, "application/json");
            }
            else
            {
                var result = JsonConvert.SerializeObject(new { IsOk = false, Message = "儲存失敗" + errMsg }, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
                return Content(result, "application/json");
            }
        }
    }
}