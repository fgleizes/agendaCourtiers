using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace agendaCabinetCourtiers2.Controllers
{
    public class AppointmentsController : Controller
    {
        // GET: Appointments
        public ActionResult Index()
        {
            return View();
        }
    }
}