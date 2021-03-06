using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using agendaCabinetCourtiers2.Models;

namespace agendaCabinetCourtiers2.Controllers
{
    public class CustomersController : Controller
    {
        private readonly agendaEntities db = new agendaEntities();

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
        public ActionResult EditCustomer(int? id)
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
            return PartialView(customer);
        }

        // action Edit
        // POST: Customers/EditCustomer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer([Bind(Include = "idCustomers,lastname,firstname,mail,phoneNumber,budget")] customers customer)
        {
            if (ModelState.IsValid)
            {
                if(customer.budget == null)
                {
                    customer.budget = 0;
                }
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                TempData["messageEditCustomer"] = "Client modifié";
                /*TempData["errorForm"] = 0;*/
                /*return RedirectToAction("ProfilCustomer/" + customer.idCustomers);*/
                return Json(new { success = true, response = "/Customers/ProfilCustomer/" + customer.idCustomers });
            }
            /*TempData["defaultBudget"] = customer.budget;*/
            /*TempData["errorForm"] = 1;*/
            /*return View("ProfilCustomer", customer);*/
            /*return PartialView(customer);*/
            return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors ).Select(x => x.ErrorMessage).ToList() });
        }

        // GET: 
        public ActionResult DeleteCustomer(int? id)
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
            return PartialView("DeleteCustomer", customer);
        }

        public ActionResult ModalDeleteAction(int? id)
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
            return PartialView("ModalDeleteContent", customer);
        }

        // action Delete
        // POST: Customers/DeleteCustomer/5
        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCustomerConfirmed(int id)
        {
            customers customer = db.customers.Find(id);
            db.customers.Remove(customer);
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
