using MyMVCProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyMVCProject.Controllers
{
    public class ReaderController : Controller
    {

        // GET: Reader
        public ActionResult Reader()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Reader(Reader reader)
        {

            MyDataBaseEntities dc = new MyDataBaseEntities();
            var model = dc.ReadingLog.ToList();
            try
            {
                dc.Reader.Add(reader);
                dc.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }

            return RedirectToAction("ReaderList", "Reader");
        }

        [HttpGet]
        public ActionResult ReaderList()
        {

            MyDataBaseEntities dc = new MyDataBaseEntities();
            var readerList = dc.Reader.ToList();
            return View(readerList);
        }

        [HttpGet]
        public ActionResult ReaderDetails(int id)
        {

            MyDataBaseEntities dc = new MyDataBaseEntities();
            var readingLogList = dc.ReadingLog.Where(a => a.ReaderId == id).ToList();
            if (readingLogList != null)
            {
                Session["ReaderName"] = readingLogList[0].ReaderName;
                Session["ReaderId"] = readingLogList[0].ReaderId;
            }
            return View(readingLogList);
        }

        [HttpGet]
        public ActionResult DeleteReader(Reader reader, int id)
        {
            string readerName = " ";
            MyDataBaseEntities dc = new MyDataBaseEntities();
            var reader_data = dc.Reader.Where(a => a.ReaderId == id).FirstOrDefault();
            if (reader_data != null)
            {
                readerName = reader_data.ReaderName;
                ViewBag.readerId = id;
                ViewBag.readerName = readerName;
            }

            return View();
        }

        [HttpPost]
        public ActionResult DeleteReader(int id)
        {
            
            MyDataBaseEntities dc = new MyDataBaseEntities();
            
            var readerId = dc.Reader.Find(id);
            dc.Reader.Remove(readerId);
            dc.SaveChanges();
            return RedirectToAction("ReaderList", "Reader");
        }


    }
}