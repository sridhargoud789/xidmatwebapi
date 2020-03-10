using ServicesAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
        //    GoogleDriveAPIHelper.FileUploadInFolder(file);
            ViewBag.Success = "File Uploaded on Google Drive";
            return View();
        }
        [HttpPost]
        public ActionResult DownloadFile()
        {
            GoogleDriveAPIHelper.DownloadGoogleFile("1RWFwIVGWH0aX7klTWyLtV2N361bkC9He");
            ViewBag.Success = "File Uploaded on Google Drive";
            return View();
        }
      
    }
}
