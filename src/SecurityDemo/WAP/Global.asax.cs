﻿using Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WAP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_OnPostAuthenticateRequest(Object sender, EventArgs e)
        {
            try
            {
                // Get a reference to the current user
                IPrincipal loggedInUser = HttpContext.Current.User;
                // Check the database for any roles and add them to the user
                var account = SecurityController.FindOrCreateAccount();
                // Add claims to the identity
                foreach (var role in account.SecurityRoles)
                {
                    var claimId = new ClaimsIdentity();
                    claimId.AddClaim(new Claim(ClaimTypes.Role, role));
                    ClaimsPrincipal.Current.AddIdentity(claimId);
                }
                HttpContext.Current.User = ClaimsPrincipal.Current;
            }
            catch (Exception ex)
            {
                // TODO: Decide what to do. As a Windows Authentication application, we don't
                // really expect an exception here.
            }

        }
    }
}
