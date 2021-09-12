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
    public class ProductsController : Controller
    {
        private KitchensWithZestEntities db = new KitchensWithZestEntities();

        // GET: Products
        public ActionResult Index()
        {
            List<Product> products = db.Products.OrderByDescending(x => x.ProductId).ToList();
            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            ViewBag.Photos = db.Photos.AsNoTracking()
                .Where(a => a.ProductId == id)
                .OrderByDescending(a => a.PhotoId).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductView productView, IEnumerable<HttpPostedFileBase> PhotoFile)
        {
            if (ModelState.IsValid)
            {
                //Upload product main photo to ~/Images/Products
                string filename = Path.GetFileNameWithoutExtension(productView.MainPhotoFile.FileName)
                    + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(productView.MainPhotoFile.FileName);
                productView.MainPhotoPath = "~/Images/Products/" + filename;
                filename = Path.Combine(Server.MapPath("~/Images/Products/"), filename);
                productView.MainPhotoFile.SaveAs(filename);

                //Pass the product inf from ProductView model to Product model
                Product product = new Product();
                product.Title = productView.Title;
                product.Description = productView.Description;
                product.MainPhotoPath = productView.MainPhotoPath;

                //Save the product inf into database table Product
                db.Products.Add(product);
                db.SaveChanges();

                //Upload multiple product gallery photos to ~/Images/Photos
                foreach (var file in productView.PhotoFile)
                {
                    if (file == null)
                    {
                        return RedirectToAction("Index");
                    }
                    string filename2 = Path.GetFileNameWithoutExtension(file.FileName)
                        + DateTime.Now.ToString("yymmssfff")
                        + Path.GetExtension(file.FileName);
                    productView.PhotoPath = "~/Images/Photos/" + filename2;
                    filename2 = Path.Combine(Server.MapPath("~/Images/Photos/"), filename2);
                    file.SaveAs(filename2);

                    //Pass the photo inf from ProductView model to Photo model
                    Photo photo = new Photo();
                    photo.ProductId = product.ProductId;
                    photo.PhotoPath = productView.PhotoPath;

                    //Save the photo inf into database table Photo
                    db.Photos.Add(photo);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
                //return RedirectToAction("Details", new { id = product.ProductId });
            }

            return View(productView);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            ViewBag.Photos = db.Photos.AsNoTracking()
                .Where(a => a.ProductId == id)
                .OrderByDescending(a => a.PhotoId).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ProductId, Product product, HttpPostedFileBase MainPhotoFile)
        {
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(MainPhotoFile.FileName)
                    + DateTime.Now.ToString("yymmssfff")
                    + Path.GetExtension(MainPhotoFile.FileName);
                product.MainPhotoPath = "~/Images/Products/" + filename;
                //filename = Path.Combine(Server.MapPath("~/Images/Products/"), filename);
                MainPhotoFile.SaveAs(Path.Combine(Server.MapPath("~/Images/Products/"), filename));

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
