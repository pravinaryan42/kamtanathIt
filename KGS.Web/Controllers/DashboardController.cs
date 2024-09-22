using KGS.Dto;
using KGS.Service;
using KGS.Web.Code.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGS.Web.Controllers
{
    [CustomAuthorization]
    public class DashboardController : BaseController
    {
        private readonly IRecordService recordService;
        // GET: Dashboard
        public DashboardController(IRecordService recordService)
        {
            this.recordService = recordService;

        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDisrictData()
        {
            List<DistrictChartModel> listData = new List<DistrictChartModel>();
            var data = recordService.GetDistrinctWiseData();
            if (data != null && data.Count() > 0)
            {
                listData = data;
            }
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserWiseData()
        {
            List<DistrictChartModel> listData = new List<DistrictChartModel>();
            var data = recordService.GetUserWiseData();
            if (data != null && data.Count() > 0)
            {
                listData = data;
            }
            return Json(listData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGPWiseData()
        {
            List<DistrictChartModel> listData = new List<DistrictChartModel>();
            var data = recordService.GetGPWiseData();
            if (data != null && data.Count() > 0)
            {
                listData = data;
            }
            return Json(listData, JsonRequestBehavior.AllowGet);
        }
    }
}