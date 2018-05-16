using MyMVCProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMVCProject.Controllers
{
    public class ReadingLogsController : Controller
    {
        // GET: ReadingLogs
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BookList()
        {
            using (MyDataBaseEntities dc = new MyDataBaseEntities())
            {
                int Id = Convert.ToInt32(Session["ReaderId"]);
                var readingLogList = dc.ReadingLog.Where(a => a.ReaderId == Id).ToList();
                //var BookList = (readingLogList.Select(a => a.BookName)).ToList();
                //dc.ReadingLog.Where(a => a.ReaderId == readingLog.ReaderId).Select(a =>a.BookName).ToList();
                //return Redirect(ReaderDetails);
                //Console.Write(BookList);
                return View("BookList", readingLogList);
            }
        }

        [HttpPost]
        public ActionResult Create(ReadingLog readingLog)
        {
            string bookName = readingLog.BookName;
            string authorName = readingLog.Author;
            readingLog.ClockIn = DateTime.Now;
            readingLog.ClockOut = DateTime.Now;
            readingLog.ReaderId = Convert.ToInt32(Session["ReaderId"]);
            readingLog.ReaderName = Convert.ToString(Session["ReaderName"]);
            int Id = Convert.ToInt32(Session["ReaderId"]);
            using (MyDataBaseEntities dc = new MyDataBaseEntities())
            {
                dc.ReadingLog.Add(readingLog);
                dc.SaveChanges();
                var readingLogList = dc.ReadingLog.Where(a => a.ReaderId == Id).ToList();
                //var BookList = (readingLogList.Select(a => a.BookName)).ToList();
                //dc.ReadingLog.Where(a => a.ReaderId == readingLog.ReaderId).Select(a =>a.BookName).ToList();
                //return Redirect(ReaderDetails);
                //Console.Write(BookList);
                return View("BookList", readingLogList);
            }
            
        }
    }
}