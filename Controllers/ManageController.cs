using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BriscollaGame.Controllers
{
    public class ManageController : Controller
    {
        // GET: Menage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit()
        {
            using (PlayersEntities players = new PlayersEntities())
            {
                Player p = players.Players.FirstOrDefault(r => r.Username == User.Identity.Name);
                return View(p);

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,Email,Win_Rate,IpAddress,Status")] Player player)
        {
            using (PlayersEntities players = new PlayersEntities())
            {
                if (ModelState.IsValid)
                {
                    players.Entry(player).State = EntityState.Modified;
                
                    players.SaveChanges();
                   
                }
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account");
            }
        }
    }
}