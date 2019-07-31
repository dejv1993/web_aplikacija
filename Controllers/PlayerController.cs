using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BriscollaGame;

namespace BriscollaGame.Controllers
{
    public class PlayerController : Controller
    {

        // GET: Player
        [CustomAuth(Roles="Player,Admin")]
        public ActionResult Index()
        {
            using (PlayersEntities players = new PlayersEntities())
            {
                var p = players.Players.ToList();
                return View(p);
            }
        }

        // GET: Player/Details/5
        [CustomAuth(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            using (PlayersEntities players = new PlayersEntities())
            {
                
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Player player = players.Players.FirstOrDefault(r => r.Id == id);
                if (player == null)
                {
                    return HttpNotFound();
                }
                return View(player);
            }
        }

        // GET: Player/Create
        [CustomAuth(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Player/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,Email,Win,Lose,IpAddress,Status")] Player player)
        {
            using (PlayersEntities players = new PlayersEntities())
            {
                if (ModelState.IsValid)
                {
                    players.Players.Add(player);
                    players.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(player);
            }
        }

        // GET: Player/Edit/5
        [CustomAuth(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (PlayersEntities players = new PlayersEntities())
            {

                Player player = players.Players.FirstOrDefault(r => r.Id == id);
                if (player == null)
                {
                    return HttpNotFound();
                }
                return View(player);
            }
        }

        // POST: Player/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
                    return RedirectToAction("Index");
                }
                return View(player);
            }
        }

        // GET: Player/Delete/5
        [CustomAuth(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
           
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            using (PlayersEntities players = new PlayersEntities())
            {
                Player player = players.Players.FirstOrDefault(r=>r.Id==id);
                if (player == null)
                {
                    return HttpNotFound();
                }
                return View(player);
            }
        }

        // POST: Player/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (PlayersEntities players = new PlayersEntities())
            {
                Player player = players.Players.Find(id);

                var games = player.Games.ToList();
                var roles = player.PlayerRoles.ToList();
                foreach(var game in games)
                {
                    player.Games.Remove(game);

                }
                var role = player.PlayerRoles.FirstOrDefault(r1 => r1.PlayerId == player.Id);
                if (role.Role.Id == 2)
                {
                    players.PlayerRoles.Remove(role);

                }
              

                players.Players.Remove(player);

                players.SaveChanges();
                return RedirectToAction("Index");
            }
        }

     
    }
}
