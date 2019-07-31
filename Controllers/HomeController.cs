using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BriscollaGame.Controllers
{
    public class HomeController : Controller
    {

            [AllowAnonymous] 
        public ActionResult Index()
        {
            using (PlayersEntities players = new PlayersEntities())
            {
                return View();
            }
            
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            
            
            ViewBag.Message = "Max number of players:2. Game Type=3 cards,4 cards";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact email:spavi43@gmail.com";

            return View();
        }
    }
}