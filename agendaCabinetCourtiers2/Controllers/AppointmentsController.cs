using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using agendaCabinetCourtiers2.Models;

namespace agendaCabinetCourtiers2.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly agendaEntities db = new agendaEntities();

        // GET: Appointments
        [ActionName("Index")]
        public ActionResult ListAppointment()
        {
            var appointments = db.appointments.Include(b => b.brokers).Include(c => c.customers);
            return PartialView("ListAppointments", appointments.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult DetailsAppointment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointments appointments = db.appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }

        // GET: Appointments/AddAppointment
        public ActionResult AddAppointment(int? id)
        {
            if (id == null)
            {
                ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "fullname");
            }
            else
            {
                TempData["getId"] = id;
                brokers broker = db.brokers.Find(id);
                if (broker == null)
                {
                    return HttpNotFound();
                }
                ViewBag.fullname = broker.fullname;
                ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "fullname", broker.idBroker);
            }
            ViewBag.idCustomers = new SelectList(db.customers, "idCustomers", "fullname");
            return View();
        }

        // POST: Appointments/AddAppointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppointment([Bind(Include = "idAppointment,dateHour,subject,idBroker,idCustomers")] appointments appointment)
        {
            if (ModelState.IsValid)
            {
                bool checkDate = db.appointments.SqlQuery("SELECT * FROM appointments WHERE idBroker = '" + appointment.idBroker + "' AND dateHour BETWEEN '" + appointment.dateHour.AddMinutes(-30).ToString("yyyyMMdd HH:mm:ss") + "' AND '" + appointment.dateHour.ToString("yyyyMMdd HH:mm:ss") + "'").ToList().Any();

                /*ViewBag.info5 = appointment.idBroker;
                ViewBag.info6 = appointment.idBroker.GetType();*/
                if (!checkDate)
                {
                    db.appointments.Add(appointment);
                    db.SaveChanges();
                    TempData["messageAddAppointment"] = "Rendez-vous enregistré";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = $"Ce courtier a déjà un rendez-vous le {appointment.dateHour:dd/MM/yyyy à HH:mm} jusqu'à {appointment.dateHour.AddMinutes(+30):HH:mm}, veuillez choisir une autre date ou un autre créneau horaire";
                }

            }

            ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "fullname", appointment.idBroker);
            ViewBag.idCustomers = new SelectList(db.customers, "idCustomers", "fullname", appointment.idCustomers);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult EditAppointment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointments appointment = db.appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "fullname", appointment.idBroker);
            ViewBag.idCustomers = new SelectList(db.customers, "idCustomers", "fullname", appointment.idCustomers);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppointment([Bind(Include = "idAppointment,dateHour,subject,idBroker,idCustomers")] appointments appointments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "lastname", appointments.idBroker);
            ViewBag.idCustomers = new SelectList(db.customers, "idCustomers", "lastname", appointments.idCustomers);
            return View(appointments);
        }

        // GET: Appointments/Delete/5
        public ActionResult DeleteAppointment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointments appointments = db.appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("DeleteAppointment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            appointments appointments = db.appointments.Find(id);
            db.appointments.Remove(appointments);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
