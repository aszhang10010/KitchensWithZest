using KitchensWithZest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KitchensWithZest.Controllers
{
    public class HomeController : Controller
    {
        private KitchensWithZestEntities db = new KitchensWithZestEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product()
        {
            List<Product> products = db.Products.OrderByDescending(x => x.ProductId).ToList();
            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult ProductDetails(int? id)
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

        public ActionResult Gallery()
        {
            List<Gallery> galleries = db.Galleries.OrderByDescending(x => x.GalleryId).ToList();
            return View(galleries);
        }

        // GET: Galleries/Details/5
        public ActionResult GalleryDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            ViewBag.Photos = db.Photos.AsNoTracking()
                .Where(a => a.GalleryId == id)
                .OrderByDescending(a => a.PhotoId).ToList();
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        public ActionResult GetStart()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        // POST: Home/Contact
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(MessageBox messageBox)
        {

            if (ModelState.IsValid)
            {

                //KitchensWithZestEntities db = new KitchensWithZestEntities();
                messageBox.SubmissionTime = DateTime.Now;
                db.MessageBoxes.Add(messageBox);
                db.SaveChanges();

                //Send success acknoledgement to view
                TempData["SuccessMsg"] = "Your messsage has been successfully sent, thank you.";
                return RedirectToAction("Contact");
            }

            return View(messageBox);
        }

        [Authorize]
        public ActionResult Admin()
        {
            return View();
        }
    }
}