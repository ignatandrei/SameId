using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

namespace SameIdAlert
{
    
    public class SameIdFilter : IActionFilter
    {
        private bool MustHaveSameIdAttribute;
        public SameIdFilter(bool mustHaveSameIdAttribute)
        {
            MustHaveSameIdAttribute = mustHaveSameIdAttribute;
        }
        public SameIdFilter()
            : this(false)
        {

        }
        static ConcurrentDictionary<string, RouteDataItems> usersPage = new ConcurrentDictionary<string, RouteDataItems>();
        static object lockMe = new object();

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {        
            
            if(filterContext.IsChildAction)
                return;

            if(filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            if (!filterContext.HttpContext.Request.IsAuthenticated)
                return;

            if(filterContext.IsChildAction)
                return;

            if(filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            

            var user = filterContext.HttpContext.User;
            if (user == null)
                return;
            if (user.Identity == null)
                return;
            string userName = user.Identity.Name;
            var skip = filterContext.ActionDescriptor.GetCustomAttributes(typeof(SameIdSkipAttribute), false).Cast<SameIdSkipAttribute>();

            if (skip != null && skip.Count() > 0)
                return;
            

            var att =  filterContext.ActionDescriptor.GetCustomAttributes(typeof (SameIdAttribute), false).Cast<SameIdAttribute>();
            
            if(MustHaveSameIdAttribute && (att == null || att.Count() == 0))
                return;
  
            //now if it is null, just make the default id one
            if(att == null || att.Count() == 0)
            {
                att=new List<SameIdAttribute>() { new SameIdAttribute()};
            }

            var routeDataItems = new RouteDataItems(filterContext.RouteData, filterContext.RouteData.Values, att.First().PropertyNames);

            usersPage.AddOrUpdate(userName, routeDataItems, (key, oldValue) => routeDataItems);
            var data = usersPage.Where(item => item.Value == routeDataItems).ToList();
            filterContext.Controller.ViewBag.Users = data;
            filterContext.Controller.ViewBag.CurrentUser = userName;
            filterContext.Controller.ViewBag.GroupName = routeDataItems.FullName;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //do nothing
        }
    }
}