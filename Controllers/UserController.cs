using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BriscollaGame.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [CustomAuth(Roles="Player,Admin")]
        public ActionResult Index()
        {
            
            using (PlayersEntities players = new PlayersEntities())
            {
                Player p = players.Players.FirstOrDefault(r => r.Username==User.Identity.Name);
                ViewBag.User = p.Username;
                return View(p);
            }
        }
        [CustomAuth(Roles = "Player,Admin")]
        public ActionResult Edit(int ? id)
        {
            if(id==0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }
            using (PlayersEntities players = new PlayersEntities())
            {
                Player player = players.Players.FirstOrDefault(r => r.Id == id);
                return View(player);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind (Include= "Id, Username, Password, Email")] Player player)
        {
            using (PlayersEntities players = new PlayersEntities())
            {
                if (ModelState.IsValid)
                {
                    players.Entry(player).State = EntityState.Modified;

                    players.SaveChanges();
                    return RedirectToAction("Index");
                }
                    return View();
                

            }
        }
        [CustomAuth(Roles = "Player,Admin")]
        public ActionResult List()
        {
           
          
            using (PlayersEntities players = new PlayersEntities())
            {
                Player player = players.Players.FirstOrDefault(r => r.Username==User.Identity.Name);
                var games = player.Games.ToList();

               
                ViewBag.played = games.Count;

                return View(games);
            }
        }
        [CustomAuth(Roles="Player,Admin")]
        public ActionResult MyStats()
        {
            int won = 0;
            int lost = 0;
            int total_games = 0;
            decimal percentage;
            using (PlayersEntities players = new PlayersEntities())
            {
                Player p = players.Players.FirstOrDefault(r => r.Username==User.Identity.Name);
                var games = p.Games.ToList();
                foreach(var g in games)
                {
                    if (g.WinnerId == p.Id)
                    {
                        won++;

                    }
                    else
                    {
                        lost++;
                    }
                }
                if (won > 0)
                {
                     percentage = (decimal)won / games.Count * 100;
                }
                else
                {
                    percentage = 0; 
                }
                p.Win_Rate = percentage;
                ViewBag.ratio = percentage;
                ViewBag.won = won;
                ViewBag.lost = lost;
                ViewBag.total = games.Count;
                players.SaveChanges();
                return View(games);
            }
        }
    }
}