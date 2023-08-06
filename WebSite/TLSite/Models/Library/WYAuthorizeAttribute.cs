using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TLSite.Models.Library
{
    public class WYAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
//             if (filterContext.Cancel && filterContext.Result is HttpUnauthorizedResult)
//             {
//                 filterContext.Result = new RedirectToRouteResult(
//                   new RouteValueDictionary {
//       { "clubShortName", filterContext.RouteData.Values[ "clubShortName" ] },
//       { "controller", "Account" },
//       { "action", "Login" },
//       { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
//     });
        }
    }
}