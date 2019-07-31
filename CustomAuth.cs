using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BriscollaGame
{
   [AttributeUsage(AttributeTargets.Method)]
    public class CustomAuth: AuthorizeAttribute
    {
        public String ViewName { get; set; }
        public CustomAuth()
        {
            ViewName = "AuthFailed";
        }
        public override void OnAuthorization(AuthorizationContext authorizationContext)
        {
            base.OnAuthorization(authorizationContext);
            isAuth(authorizationContext);

        }
        public void isAuth(AuthorizationContext authorizationContext)
        { 

            if(authorizationContext.Result==null)
            {
                return;
            }
            if(authorizationContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ViewDataDictionary dict = new ViewDataDictionary();
                dict.Add("Error", "You dont have premisson");
                var result = new ViewResult { ViewName = this.ViewName, ViewData = dict };
                authorizationContext.Result = result;
            }
        }
    }
}