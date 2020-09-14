using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SecurityWebsite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        //is triggered with every request to the website
        protected void Application_AuthenticateRequest(object sender, EventArgs args)
        {
            if(User != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    IQueryable<Role> userRoles = new BusinessLogic.UsersBL().GetRolesForUser(Context.User.Identity.Name);

                    GenericPrincipal gp = new GenericPrincipal(Context.User.Identity, userRoles.Select(x => x.Title).ToArray());

                    Context.User = gp;
                }
            }
        }
    }
}
