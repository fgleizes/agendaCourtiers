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
    public class CustomersController : Controller
    {
        private agendaEntities db = new agendaEntities();

        // GET: Customers
        [ActionName("ListCustomers")]
        public ActionResult Index()
        {
            IEnumerable<customers> customers = db.customers.SqlQuery("SELECT iDcustomers,lastname,firstname,mail,phoneNumber,budget FROM customers ORDER BY lastname ASC").ToList();
            return View(customers);
        }

        // Action Create
        // GET: Customers/AddCustomer
        public ActionResult AddCustomer()
        {
            return View();
        }

        // Action Create
        // POST: Customers/AddCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomer([Bind(Include = "idCustomers,lastname,firstname,mail,phoneNumber,budget")] customers customer)
        {
            if (ModelState.IsValid)
            {
                db.customers.Add(customer);
                db.SaveChanges();
                TempData["messageAddCustomer"] = "Client ajouté";
                return RedirectToAction("ListCustomers");
            }
            TempData["defaultBudget"] = customer.budget;
            return View(customer);
        }

        // Action Details
        // GET: Customers/ProfilCustomer/5
        public ActionResult ProfilCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customers customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/EditCustomer/5
        /*public ActionResult EditCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customers customers = db.customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }*/

        // action Edit
        // POST: Customers/EditCustomer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer([Bind(Include = "idCustomers,lastname,firstname,mail,phoneNumber,budget")] customers customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                TempData["messageEditCustomer"] = "Client modifié";
                TempData["errorForm"] = 0;
                return RedirectToAction("ProfilCustomer/" + customer.idCustomers);
            }
            TempData["defaultBudget"] = customer.budget;
            TempData["errorForm"] = 1;
            return View("ProfilCustomer", customer);
        }

        // action Delete
        // GET: Customers/DeleteCustomer/5
        public ActionResult DeleteCustomer(int id)
        {
            customers customers = db.customers.Find(id);
            db.customers.Remove(customers);
            db.SaveChanges();
            TempData["messageDeleteCustomer"] = "Client supprimé";
            return RedirectToAction("ListCustomers");
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
