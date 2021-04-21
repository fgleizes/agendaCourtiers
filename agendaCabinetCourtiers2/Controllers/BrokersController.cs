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

        [ActionName("Index")]
        public ActionResult ListBrokers()
        {
            IEnumerable<brokers> brokers = db.brokers.ToList();
            return View("ListBrokers", brokers);
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
                return RedirectToAction("Index");
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

        // GET:brokers/EditBroker/5
        public ActionResult EditBroker(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brokers broker = db.brokers.Find(id);
            if (broker == null)
            {
                return HttpNotFound();
            }
            return PartialView(broker);
        }

        // action Edit
        // POST: brokers/EditBroker/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBroker([Bind(Include = "idBroker,lastname,firstname,mail,phoneNumber")] brokers broker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(broker).State = EntityState.Modified;
                db.SaveChanges();
                TempData["messageEditBroker"] = "Courtier modifié";
                /*TempData["errorForm"] = 0;*/
                /*return RedirectToAction("ProfilBroker/" + broker.idBroker);*/
                return Json(new { success = true, response = "/brokers/ProfilBroker/" + broker.idBroker });
            }
            /*TempData["errorForm"] = 1;*/
            /*return View("ProfilBroker", broker);*/
            return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() });
        }

        // GET
        public ActionResult DeleteBroker(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brokers broker = db.brokers.Find(id);
            if (broker == null)
            {
                return HttpNotFound();
            }
            return PartialView("DeleteBroker", broker);
        }

        /*public ActionResult DeleteBroker(int Id)
        {
            brokers broker = db.brokers.Find(Id);
            db.brokers.Remove(broker);
            db.SaveChanges();
            TempData["messageDeleteBroker"] = "Courtier supprimé";
            return RedirectToAction("Index");
        }*/

        // action Delete
        // POST: Customers/DeleteCustomer/5
        [HttpPost, ActionName("DeleteBroker")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBrokerConfirmed(int id)
        {
            brokers broker = db.brokers.Find(id);
            db.brokers.Remove(broker);
            db.SaveChanges();
            TempData["messageDeleteBroker"] = "Courtier supprimé";
            return RedirectToAction("Index");
        }
    }
}