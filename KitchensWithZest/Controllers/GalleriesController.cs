using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KitchensWithZest.Models.ViewModels;
using KitchensWithZest.Models;

namespace KitchensWithZest.Controllers
{
    public class GalleriesController : Controller
    {
        private KitchensWithZestEntities db = new KitchensWithZestEntities();

        // GET: Galleries
        public ActionResult Index()
        {
            List<Gallery> galleries = db.Galleries.OrderByDescending(x => x.GalleryId).ToList();
            return View(galleries);
        }

        // GET: Galleries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            ViewBag.Photos = db.Photos.AsNoTracking()
                .Where(a => a.GalleryId == id)
                .OrderByDescending(a=>a.PhotoId).ToList();
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // GET: Galleries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GalleryView galleryView, IEnumerable<HttpPostedFileBase> PhotoFile)
        {
            if (ModelState.IsValid)
            {
                //Upload gallery main photo to ~/Images/Galleries
                string filename = Path.GetFileNameWithoutExtension(galleryView.MainPhotoFile.FileName)
                    + DateTime.Now.ToString("yymmssfff")
                    + Path.GetExtension(galleryView.MainPhotoFile.FileName);
                galleryView.MainPhotoPath = "~/Images/Galleries/" + filename;
                filename = Path.Combine(Server.MapPath("~/Images/Galleries/"), filename);
                galleryView.MainPhotoFile.SaveAs(filename);

                //Pass the gallery inf from ProductView model to Gallery model
                Gallery gallery = new Gallery();
                gallery.Title = galleryView.Title;
                gallery.Description = galleryView.Description;
                gallery.MainPhotoPath = galleryView.MainPhotoPath;

                //Save the gallery inf into database table Gallery
                db.Galleries.Add(gallery);
                db.SaveChanges();

                //Upload multiple gallery photos to ~/Images/Galleries
                foreach (var file in galleryView.PhotoFile)
                {
                    if (file == null)
                    {
                        return RedirectToAction("Index");
                    }
                    string filename2 = Path.GetFileNameWithoutExtension(file.FileName)
                        + DateTime.Now.ToString("yymmssfff")
                        + Path.GetExtension(file.FileName);
                    galleryView.PhotoPath = "~/Images/Photos/" + filename2;
                    filename2 = Path.Combine(Server.MapPath("~/Images/Photos/"), filename2);
                    file.SaveAs(filename2);

                    //Pass the photo inf from ProductView model to Gallery model
                    Photo photo = new Photo();
                    photo.GalleryId = gallery.GalleryId;
                    photo.PhotoPath = galleryView.PhotoPath;

                    //Save the photo inf into database table Gallery
                    db.Photos.Add(photo);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(galleryView);
        }

        // GET: Galleries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int GalleryId, Gallery gallery, HttpPostedFileBase MainPhotoFile)
        {
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(MainPhotoFile.FileName)
                    + DateTime.Now.ToString("yymmssfff")
                    + Path.GetExtension(MainPhotoFile.FileName);
                gallery.MainPhotoPath = "~/Images/Galleries/" + filename;
                MainPhotoFile.SaveAs(Path.Combine(Server.MapPath("~/Images/Galleries/"), filename));

                db.Entry(gallery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gallery);
        }

        // GET: Galleries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gallery gallery = db.Galleries.Find(id);
            db.Galleries.Remove(gallery);
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
