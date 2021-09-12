using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KitchensWithZest.Models;

namespace KitchensWithZest.Controllers
{
    public class ProductPhotosController : Controller
    {
        private KitchensWithZestEntities db = new KitchensWithZestEntities();

        // GET: ProductPhotos/Create
        public ActionResult Create(int ProductId, string Title)
        {
            ViewBag.ProductId = ProductId;
            ViewBag.ProductTitle = Title;
            Photo photo = new Photo()
            {
                ProductId = ProductId
            };
            return View(photo);
        }

        // POST: ProductPhotos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Photo photo, IEnumerable<HttpPostedFileBase> PhotoFile, int ProductId)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in photo.PhotoFile)
                {
                    if (file == null)
                    {
                        return RedirectToAction("Index");
                    }
                    string filename = Path.GetFileNameWithoutExtension(file.FileName)
                        + DateTime.Now.ToString("yymmssfff")
                        + Path.GetExtension(file.FileName);
                    photo.PhotoPath = "~/Images/Photos/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Images/Photos/"), filename);
                    file.SaveAs(filename);

                    //Save the photo inf into database table Photo
                    db.Photos.Add(photo);
                    db.SaveChanges();
                }

                return RedirectToAction("Details", "Products", new { id = photo.ProductId});
            }

            return View(photo);
        }

        // GET: ProductPhotos/Edit/5
        public ActionResult Edit(int? id, int ProductId, string ProductTitle)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = ProductId;
            return View(photo);
        }

        // POST: ProductPhotos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, int ProductId, Photo photo, HttpPostedFileBase PhotoFile)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(PhotoFile.FileName)
                    + DateTime.Now.ToString("yymmssfff")
                    + Path.GetExtension(PhotoFile.FileName);
                photo.PhotoPath = "~/Images/Photos/" + filename;
                PhotoFile.SaveAs(Path.Combine(Server.MapPath("~/Images/Photos/"), filename));

                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Products", new { id = photo.ProductId });
            }
            ViewBag.ProductId = ProductId;
            return View(photo);
        }

        // GET: ProductPhotos/Delete/5
        public ActionResult Delete(int? id, int ProductId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: ProductPhotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int ProductId)
        {
            Photo photo = db.Photos.Find(id);
            var ProId = photo.ProductId;
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("Details", "Products", new { id = ProId });
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
