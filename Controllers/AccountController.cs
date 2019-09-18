using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BriscollaGame.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
namespace BriscollaGame.Controllers
{
    public class AccountController : Controller
    {
        public IAuthenticationManager Authentication => HttpContext.GetOwinContext().Authentication;
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {

                return RedirectToAction("index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")] Player player)
        {
            if (ModelState.IsValid)
            {




                using (PlayersEntities players = new PlayersEntities())
                {


                    Player user = players.Players.FirstOrDefault(r => r.Password == player.Password && r.Email == player.Email);

                    if (user != null)
                    {
                        var role = user.PlayerRoles.FirstOrDefault(r => r.Role.Name == "Admin");
                        var roleUser = user.PlayerRoles.FirstOrDefault(r => r.Role.Name == "Player");
                        if (role != null)
                        {
                            




                                FormsAuthentication.SetAuthCookie(user.Username, false);

                                     return   RedirectToAction("index", "Home");


                            

                        }
                        else if (roleUser != null)
                        {
                           
                                FormsAuthentication.SetAuthCookie(user.Username, false);
                               return  RedirectToAction("about", "Home");

                            




                        }




                    }



                }
            }
        
                return View(player);
            
            
        }
        
        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include="Username,Password,Email")] Player player)
        {
            if(ModelState.IsValid)
            {
                using (PlayersEntities players =new PlayersEntities())
                {
                    Player p = players.Players.FirstOrDefault(r => r.Username == player.Username || r.Email == player.Email);
                    if (p != null)
                    {

                        ViewBag.Error = "Player with that username or mail already exist";
                        return View(player);
                    }
                    else 
                    {
                       
                        if (player.Password.Length< 6)
                        {
                            ViewBag.Pass = "password must be at least 6 chars long";
                            return View(player);
                        }
                        else
                        {

                           
                            var role = new PlayerRole { PlayerId = player.Id, RoleId= 2 };
                            player.PlayerRoles.Add(role);
                            player.Win_Rate = (decimal)0.0;
                            player.Status = "Offline";
                            players.Players.Add(player);

                            players.SaveChanges();
                         return   RedirectToAction("Login","Account");
                        }
                    }
                }
            }

            return View(player);

        }


        [HttpPost]
        [Authorize(Roles= "Admin,Player")]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account");

            }
           
        

    }
}