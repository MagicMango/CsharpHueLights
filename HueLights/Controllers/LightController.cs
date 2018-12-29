using HueLights.Models.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HueLights.Controllers
{
    public class LightController : Controller
    {
        private HueHandler lightHandler = new HueHandler();

        [HttpGet]
        public ActionResult Index(int id, string color, string mode)
        {
            var result = lightHandler.ControlLight(id, color, mode);
            if (result && mode != "disco")
            {
                return Content("Changed light to color: " + color + " with mode: " + mode, "text/plain");
            }
            else if(result && mode == "disco")
            {
                return Content("Successfully initiated disco mode", "text/plain");
            }
            else
            {
                return Content("Could not change color.", "text/plain");
            }
        }
    }
}