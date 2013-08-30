using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using SameId.SameIdAlert;

namespace SameIdAlert
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SameIdSkipAttribute : Attribute
    {

    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SameIdAttribute : Attribute
    {

        public SameIdAttribute(params string[] propertyNames)
        {
            if (propertyNames == null || propertyNames.Length == 0)
            {
                this.PropertyNames = new string[1] { "id" };
            }
            else
            {
                this.PropertyNames = propertyNames;
            }
        }
        public string[] PropertyNames;
    }

    public class RouteDataItems : IEquatable<RouteDataItems>
    {
        public RouteDataItems(RouteData rd, IDictionary<string, object> routeValues, string[] parameterNames)
        {
            this.ActionName = rd.Values["action"].ToString();
            this.ControllerName = rd.Values["controller"].ToString();
            this.AreaName = (rd.Values["area"] ?? "").ToString();
            this.Arguments = string.Join("&", routeValues.Where(item => parameterNames.Contains(item.Key)).Select(item => string.Format("{0}={1}", item.Key, (item.Value == null) ? "" : item.ToString())));
            this.Arguments = this.Arguments;
        }
        public string AreaName;
        public string ControllerName;
        public string ActionName;

        public string Arguments;

        public string FullName
        {
            get
            {
                return string.Format("{0} {1} {2} {3}", AreaName, ControllerName, ActionName, Arguments);
            }
        }

        public bool Equals(RouteDataItems other)
        {
            if (other == null)
                return false;

            if (this.FullName != other.FullName)
                return false;

            if (this.AreaName != other.AreaName)
                return false;
            if (this.ControllerName != other.ControllerName)
                return false;

            if (this.ActionName != other.ActionName)
                return false;


            if (this.Arguments != other.Arguments)
                return false;

            return true;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            RouteDataItems RouteDataItemsObj = obj as RouteDataItems;
            if (RouteDataItemsObj == null)
                return false;
            else
                return Equals(RouteDataItemsObj);
        }

        public override int GetHashCode()
        {
            return (this.AreaName + this.ControllerName + this.ActionName + this.Arguments).GetHashCode();
        }

        public static bool operator ==(RouteDataItems RouteDataItems1, RouteDataItems RouteDataItems2)
        {
            if ((object)RouteDataItems1 == null || ((object)RouteDataItems2) == null)
                return Object.Equals(RouteDataItems1, RouteDataItems2);

            return RouteDataItems1.Equals(RouteDataItems2);
        }

        public static bool operator !=(RouteDataItems RouteDataItems1, RouteDataItems RouteDataItems2)
        {
            if (RouteDataItems1 == null || RouteDataItems2 == null)
                return !Object.Equals(RouteDataItems1, RouteDataItems2);

            return !(RouteDataItems1.Equals(RouteDataItems2));
        }


    }
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