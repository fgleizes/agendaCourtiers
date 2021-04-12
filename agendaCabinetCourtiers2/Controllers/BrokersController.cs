using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using agendaCabinetCourtiers2.Models;

namespace agendaCabinetCourtiers2.Controllers
{
    public class BrokersController : Controller
    {
        private readonly agendaEntities db = new agendaEntities();

        [ActionName("ListBrokers")]
        public ActionResult Index()
        {
            IEnumerable<brokers> brokers = db.brokers.ToList();
            return View(brokers);

            /*return View(db.brokers.ToList());*/
        }

        // GET: AddBroker
        public ActionResult AddBroker()
        {
            return View();
        }

        // POST: AddBroker
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBroker([Bind(Include = "idBroker,lastname,firstname,mail,phoneNumber")] brokers broker)
        {
            if (ModelState.IsValid)
            {
                db.brokers.Add(broker);
                db.SaveChanges();
                TempData["messageAddBroker"] = "Courtier ajouté";
                return RedirectToAction("ListBrokers");
            }

            return View(broker);
        }

        // Détails d'un Broker
        // GET: Brokers/ProfilBroker/5
        public ActionResult ProfilBroker( int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brokers broker = db.brokers.Find(Id);
            if (broker == null)
            {
                return HttpNotFound();
            }
            return View(broker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBroker(brokers broker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(broker).State = EntityState.Modified;
                db.SaveChanges();
                TempData["messageEditBroker"] = "Courtier modifié";
                TempData["errorForm"] = 0;
                return RedirectToAction("ProfilBroker/" + broker.idBroker);
            }
            TempData["errorForm"] = 1;
            return View("ProfilBroker", broker);
        }

        public ActionResult DeleteBroker(int Id)
        {
            brokers broker = db.brokers.Find(Id);
            db.brokers.Remove(broker);
            db.SaveChanges();
            TempData["messageDeleteBroker"] = "Courtier supprimé";
            return RedirectToAction("ListBrokers");
        }
    }
}