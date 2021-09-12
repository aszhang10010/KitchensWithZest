using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KitchensWithZest.Models;

namespace KitchensWithZest.Controllers
{
    public class MessageBoxesController : Controller
    {
        private KitchensWithZestEntities db = new KitchensWithZestEntities();

        // GET: MessageBoxes
        public ActionResult Index()
        {
            return View(db.MessageBoxes.OrderByDescending(x=>x.MessageId).ToList());
        }

        // GET: MessageBoxes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBox messageBox = db.MessageBoxes.Find(id);
            if (messageBox == null)
            {
                return HttpNotFound();
            }
            return View(messageBox);
        }



        // GET: MessageBoxes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBox messageBox = db.MessageBoxes.Find(id);
            if (messageBox == null)
            {
                return HttpNotFound();
            }
            return View(messageBox);
        }

        // POST: MessageBoxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MessageBox messageBox = db.MessageBoxes.Find(id);
            db.MessageBoxes.Remove(messageBox);
            db.SaveChanges();
            return RedirectToAction("Index");
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
