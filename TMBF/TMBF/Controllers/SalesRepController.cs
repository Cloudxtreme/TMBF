﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TMBF.Models;
using TMBF.DAL;
using System.Web.Security;
using System.Diagnostics;
using PagedList;

namespace TMBF.Controllers
{
    [AdminR]
    public class SalesRepController : Controller
    {
        private TelecomContext db = new TelecomContext();

        // GET: /SalesRep/
        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            var salesRep = from s in db.SalesReps
                         select s;
            switch (sortOrder)
            {
                default:
                    salesRep = salesRep.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(salesRep.ToPagedList(pageNumber, pageSize));
        }        

        // GET: /SalesRep/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesRep salesrep = db.SalesReps.Find(id);
            if (salesrep == null)
            {
                return HttpNotFound();
            }
            return View(salesrep);
        }

        // GET: /SalesRep/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /SalesRep/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,FirstName,LastName,Password")] SalesRep salesrep)
        {
            long max = 1;
            foreach(var sr in db.SalesReps){
                if (max < sr.ID) max = sr.ID;
            }
            // Debug.WriteLine(max);
            salesrep.ID = max + 1;
            if (ModelState.IsValid)
            {
                salesrep.role = TMBF.Models.User.Role.SalesRep;
                db.SalesReps.Add(salesrep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salesrep);
        }

        // GET: /SalesRep/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesRep salesrep = db.SalesReps.Find(id);
            if (salesrep == null)
            {
                return HttpNotFound();
            }
            return View(salesrep);
        }

        // POST: /SalesRep/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,FirstName,LastName,Password")] SalesRep salesrep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesrep).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salesrep);
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
