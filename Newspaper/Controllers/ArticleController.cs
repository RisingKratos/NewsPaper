using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newspaper.DAL;
using Newspaper.Models;
using PagedList;

namespace Newspaper.Controllers
{
    //Delete MyType later, unused because somehow it doesn't work with linq, unusual, unexpected constraints
    public class MyType 
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Views { get; set; }
        public Author Author { get; set; }
        public Category Category { get; set; }
        public int ID { get; set; }
    }
    public class ArticleController : Controller
    {
        private NewsContext db = new NewsContext();

        // GET: Article
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //@Html.DisplayNameFor(model => model.Views)    как правильно решить?
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParameter = String.IsNullOrEmpty(sortOrder) ? "TitleDesc" : "";
            ViewBag.DateSortParm = sortOrder == "CreatedDate" ? "CreatedDateDesc" : "CreatedDate";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var articles = db.Articles.Include(a => a.Author).Include(a => a.Category);

            if (!String.IsNullOrWhiteSpace(searchString) && !String.IsNullOrWhiteSpace(searchString)) 
            {
                articles = articles.Where(student => student.Title.Contains(searchString) 
                    || student.Content.Contains(searchString) 
                    || student.Author.FirstName.Contains(searchString) || student.Author.LastName.Contains(searchString) 
                    || student.Category.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Title":
                    articles = articles.OrderBy(order => order.Title);
                    break;
                case "TitleDesc":
                    articles = articles.OrderByDescending(order => order.Title);
                    break;
                case "Content":
                    articles = articles.OrderBy(order => order.Content);
                    break;
                case "ContentDesc":
                    articles = articles.OrderByDescending(order => order.Content);
                    break;
                case "CreatedDate":
                    articles = articles.OrderBy(order => order.CreatedDate);
                    break;
                case "CreatedDateDesc":
                    articles = articles.OrderByDescending(order => order.CreatedDate);
                    break;
                case "Views":
                    articles = articles.OrderBy(order => order.Views);
                    break;
                case "ViewsDesc":
                    articles = articles.OrderByDescending(order => order.Views);
                    break;
                default:
                    articles = articles.OrderBy(order => order.Title);
                    break;
            }

            articles.ToList().ForEach(article => { article.Content = article.Content.ToString().Substring(0, 25); } );

            int pageSize = 2;
            // if page has number - return it, else - 1, because page is nullable type
            int pageNumber = (page ?? 1);
            return View(articles.ToPagedList(pageNumber, pageSize));
        }

        // GET: Article/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            article.Views += 1;
            db.Entry(article).State = EntityState.Modified;
            db.SaveChanges();
            return View(article);
        }

        // GET: Article/Create
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Article/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Content,CreatedDate,Views,AuthorID,CategoryID")] Article article)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Articles.Add(article);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /*dex*/)
            {
                //ModelState.AddModelError("", dex.TargetSite.ToString());
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName", article.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", article.CategoryID);
            return View(article);
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);

            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName", article.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", article.CategoryID);
            return View(article);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Content,CreatedDate,Views,AuthorID,CategoryID")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName", article.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", article.CategoryID);
            return View(article);
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Article article = db.Articles.Find(id);
                db.Articles.Remove(article);
                db.SaveChanges();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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
