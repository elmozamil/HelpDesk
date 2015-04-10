using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HelpDesk.Models;
using AttributeRouting.Web.Mvc;
using System.IO;

namespace HelpDesk.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private HelpDeskEntities db = new HelpDeskEntities();

        // GET: Tickets
        public async Task<ActionResult> Index()
        {
            ViewBag.SiteNo = new SelectList(db.Sites, "SiteNo", "SiteName");
            ViewBag.Status = new SelectList(db.Status, "Status1", "Status1");


            List<SelectListItem> priority = new List<SelectListItem>();


            priority.Add(new SelectListItem { Text = "High", Value = "High" });
            priority.Add(new SelectListItem { Text = "Normal", Value = "Normal" });
            priority.Add(new SelectListItem { Text = "Low", Value = "Low" });
            ViewBag.Priority = priority;

            List<SelectListItem> impact = new List<SelectListItem>();

            impact.Add(new SelectListItem { Text = "Critical", Value = "Critical" });
            impact.Add(new SelectListItem { Text = "High", Value = "High" });
            impact.Add(new SelectListItem { Text = "Normal", Value = "Normal" });
            impact.Add(new SelectListItem { Text = "Low", Value = "Low" });
            ViewBag.Impact = impact;


            var tickets = db.Tickets.Include(t => t.Category).Include(t => t.Employee).Include(t => t.Site).Include(t => t.Status1).Include(t => t.User);
            return View(await tickets.ToListAsync());
        }


        [GET("MyTickets")]
        public async Task<ActionResult> MyTickets(string userName)
        {
            int empID = (from emp in db.Employees.Where(e => e.Email.Substring(0, e.Email.IndexOf("@")) == userName) select emp.EmployeeCode).FirstOrDefault();


            var tickets = db.Tickets.Include(t => t.Category).Include(t => t.Employee).Include(t => t.Site).Include(t => t.Status1).Include(t => t.User).Where(emp => emp.Issuer == empID);
            return View(await tickets.ToListAsync());
        }

        [GET("AssignedTickets")]
        public ActionResult AssignedTickets(string userName, string Status, int? SiteNo, string Priority, string Impact)
        {
            ViewBag.SiteNo = new SelectList(db.Sites, "SiteNo", "SiteName");
            ViewBag.Status = new SelectList(db.Status, "Status1", "Status1");
            

            List<SelectListItem> priority = new List<SelectListItem>();


            priority.Add(new SelectListItem { Text = "High", Value = "High" });
            priority.Add(new SelectListItem { Text = "Normal", Value = "Normal" });
            priority.Add(new SelectListItem { Text = "Low", Value = "Low" });
            ViewBag.Priority = priority;

            List<SelectListItem> impact = new List<SelectListItem>();

            impact.Add(new SelectListItem { Text = "Critical", Value = "Critical" });
            impact.Add(new SelectListItem { Text = "High", Value = "High" });
            impact.Add(new SelectListItem { Text = "Normal", Value = "Normal" });
            impact.Add(new SelectListItem { Text = "Low", Value = "Low" });
            ViewBag.Impact = impact;


                        
            var tickets = db.Tickets.Include(t => t.Category).Include(t => t.Employee).Include(t => t.Site).Include(t => t.Status1).Include(t => t.User).Where(emp => emp.Assignee == userName);

            if (SiteNo > 0)
            {
                tickets.Where(t => t.SiteNo == SiteNo);
            }
            if (!String.IsNullOrEmpty(Priority))
            {
                tickets.Where(t => t.Priority == Priority);
            }

            if (!String.IsNullOrEmpty(Impact))
            {
                tickets.Where(t => t.Impact == Impact);
            }

            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [GET("NewTicket")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Type1");

            ViewBag.Issuer = new SelectList(db.Employees, "EmployeeCode", "EmployeeName");
            ViewBag.SiteNo = new SelectList(db.Sites, "SiteNo", "SiteName");
            ViewBag.Status = new SelectList(db.Status, "Status1", "Status1");
            ViewBag.Assignee = new SelectList(db.Users, "userName", "Name");

            List<SelectListItem> priority = new List<SelectListItem>();


            priority.Add(new SelectListItem { Text = "High", Value = "High" });
            priority.Add(new SelectListItem { Text = "Normal", Value = "Normal" });
            priority.Add(new SelectListItem { Text = "Low", Value = "Low" });
            ViewBag.Priority = priority;

            List<SelectListItem> impact = new List<SelectListItem>();

            impact.Add(new SelectListItem { Text = "Critical", Value = "Critical" });
            impact.Add(new SelectListItem { Text = "High", Value = "High" });
            impact.Add(new SelectListItem { Text = "Normal", Value = "Normal" });
            impact.Add(new SelectListItem { Text = "Low", Value = "Low" });
            ViewBag.Impact = impact;



            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [POST("NewTicket")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Ticket ticket, IEnumerable<HttpPostedFileBase> Attachments)
        {
            if (String.IsNullOrEmpty(ticket.ActivePhone))
                ticket.ActivePhone = (from emp in db.Employees.Where(em => em.EmployeeCode == ticket.Issuer) select emp.PhoneNo).First();

            if (ModelState.IsValid)
            {
                try
                {
                    ticket.CreatedDate = DateTime.Now;
                    ticket.Status = "Open";
                    db.Tickets.Add(ticket);
                    await db.SaveChangesAsync();
                    long ticketno = ticket.TicketNo;

                    if (Attachments.First() != null)
                    {
                        string strMappath = Server.MapPath("~/Attachments/" + ticketno.ToString());

                        if (!Directory.Exists(strMappath))
                        {
                            Directory.CreateDirectory(strMappath);
                        }

                        foreach (var file in Attachments)
                        {
                            if (file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/Attachments/" + ticketno.ToString()), fileName);
                                file.SaveAs(path);
                            }
                            //For byte array data
                            //byte[] data;
                            //using (Stream inputStream = file.InputStream)
                            //{
                            //    MemoryStream memoryStream = inputStream as MemoryStream;
                            //    if (memoryStream == null)
                            //    {
                            //        memoryStream = new MemoryStream();
                            //        inputStream.CopyTo(memoryStream);
                            //    }
                            //    data = memoryStream.ToArray();
                            //    TicketAttachment attach = new TicketAttachment();
                            //    attach.fileName = "~/Attachments/" + ticketno.ToString() + "/" + file.FileName.ToString();
                            //    attach.fileType = Path.GetExtension(file.FileName).ToString();
                            //    attach.TicketNo = ticketno;
                            //    attach.Attachment = data;
                            //    db.TicketAttachments.Add(attach);
                            //    await db.SaveChangesAsync();
                            //}
                        }
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                }
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Type1");

            ViewBag.Issuer = new SelectList(db.Employees, "EmployeeCode", "EmployeeName");
            ViewBag.SiteNo = new SelectList(db.Sites, "SiteNo", "SiteName");
            ViewBag.Status = new SelectList(db.Status, "Status1", "Status1");
            ViewBag.Assignee = new SelectList(db.Users, "userName", "Name");

            List<SelectListItem> priority = new List<SelectListItem>();


            priority.Add(new SelectListItem { Text = "High", Value = "High" });
            priority.Add(new SelectListItem { Text = "Normal", Value = "Normal" });
            priority.Add(new SelectListItem { Text = "Low", Value = "Low" });
            ViewBag.Priority = priority;

            List<SelectListItem> impact = new List<SelectListItem>();

            impact.Add(new SelectListItem { Text = "Critical", Value = "Critical" });
            impact.Add(new SelectListItem { Text = "High", Value = "High" });
            impact.Add(new SelectListItem { Text = "Normal", Value = "Normal" });
            impact.Add(new SelectListItem { Text = "Low", Value = "Low" });
            ViewBag.Impact = impact;

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.Issuer = new SelectList(db.Employees, "EmployeeCode", "EmployeeName", ticket.Issuer);
            ViewBag.SiteNo = new SelectList(db.Sites, "SiteNo", "SiteName", ticket.SiteNo);
            ViewBag.Status = new SelectList(db.Status, "Status1", "Status1", ticket.Status);
            ViewBag.Assignee = new SelectList(db.Users, "userName", "Name", ticket.Assignee);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Ticket ticket, string txtSolution, string txtuserName)
        {
            Ticket oldTicket = db.Tickets.Find(ticket.TicketNo);
            oldTicket.Status = ticket.Status;
            if (ModelState.IsValid)
            {
                db.Entry(oldTicket).State = EntityState.Modified;
                await db.SaveChangesAsync();

                StatusHistory StatHist = new StatusHistory() { Status = ticket.Status, Description = txtSolution, UserName = txtuserName, TicketNo = ticket.TicketNo, ChangeDate = DateTime.Now };
                db.StatusHistories.Add(StatHist);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.Issuer = new SelectList(db.Employees, "EmployeeCode", "EmployeeName", ticket.Issuer);
            ViewBag.SiteNo = new SelectList(db.Sites, "SiteNo", "SiteName", ticket.SiteNo);
            ViewBag.Status = new SelectList(db.Status, "Status1", "Status1", ticket.Status);
            ViewBag.Assignee = new SelectList(db.Users, "userName", "Name", ticket.Assignee);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [GET("GetCategories")]
        [HttpGet]
        public JsonResult GetCategories(int id)
        {
            var categories = (from cat in db.Categories.Where(c => c.TypeID == id) select new { cat.CategoryID, cat.CategoryName }).ToList();
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [GET("GetIssuerEmail")]
        [HttpGet]
        public JsonResult GetIssuerEmail(int empCode)
        {
            var empEmails = (from emp in db.Employees.Where(em => em.EmployeeCode == empCode) select new { emp.Email, emp.PhoneNo }).ToList();
            return Json(empEmails, JsonRequestBehavior.AllowGet);
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
