using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Collections.Concurrent;
using System.Linq;

namespace SameIDAlert
{
    public class SameIdFilter : ActionFilterAttribute
    {
        static ConcurrentDictionary<string, RouteDataItems> usersPage = new ConcurrentDictionary<string, RouteDataItems>();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cnt = context.HttpContext;
            if (cnt?.User?.Identity == null || !cnt.User.Identity.IsAuthenticated)
            {
                return;
            }
            var ad = context.ActionDescriptor as ControllerActionDescriptor;
            if (ad == null)
                return ;
            var cont = context.Controller as Controller;
            if (cont == null)
                return;
            var user = context.HttpContext.User;
            string userName = user.Identity.Name;

            var skip = ad.MethodInfo
                .GetCustomAttributes(typeof(SameIdSkipAttribute), false)
                .Cast<SameIdSkipAttribute>()
                .ToArray();
            if (skip?.Length > 0)
                return;

            var att = ad
                .MethodInfo
                .GetCustomAttributes(typeof(SameIdAttribute), false)
                .Cast<SameIdAttribute>()
                .ToArray();

            if (!(att?.Length > 0))
                return;

            var routeDataItems = new RouteDataItems(cnt.GetRouteData(), cnt.GetRouteData().Values, att.First().PropertyNames);

            usersPage.AddOrUpdate(userName, routeDataItems, (key, oldValue) => routeDataItems);
            var data = usersPage.Where(item => item.Value == routeDataItems).ToList();


            cont.ViewBag.Users = data;
            cont.ViewBag.CurrentUser = userName;
            cont.ViewBag.GroupName = routeDataItems.FullName.ToLowerInvariant();
            base.OnActionExecuting(context);
        }
                
    }

}
