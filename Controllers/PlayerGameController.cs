using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BriscollaGame.Controllers
{
    public class PlayerGameController : Controller
    {
        // GET: PlayerGame
        [Authorize(Roles="Admin")]

        public ActionResult Index()
        {
           
            return View();
        }
        [AllowAnonymous]
        public ActionResult TopPlayers()
        {
           
            int wins = 0;
            using (PlayersEntities players = new PlayersEntities())
            {
                var player = players.Players.ToList();
               
                var game = players.Games.ToList();
                foreach (var pl in player)
                {
                   
                        foreach (var g in game)
                        {
                            if (pl.Id == g.WinnerId)
                            {
                                wins++;
                            }



                        }


                    if (pl.Games.Count > 0)
                    {
                        pl.Win_Rate = (decimal)wins / pl.Games.Count * 100;
                        wins = 0;
                    }
                    else
                    {
                        pl.Win_Rate =(decimal)0.0;
                        wins = 0;
                    }
                    players.SaveChanges();
                   
                }
              
                var p = players.Players.OrderBy(r=>r.Games.Count).OrderByDescending(r => r.Win_Rate).Take(3).ToList();
                return View(p);
            }
        }
    }
}