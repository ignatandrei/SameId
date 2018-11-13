using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SameIDAlert
{
    public class RouteDataItems : IEquatable<RouteDataItems>
    {
        public RouteDataItems(RouteData rd, IDictionary<string, object> routeValues, string[] parameterNames)
        {
            this.ActionName = rd.Values["action"].ToString();
            this.ControllerName = rd.Values["controller"].ToString();
            this.AreaName = (rd.Values["area"] ?? "").ToString();
            this.Arguments = string.Join("&", routeValues.Where(item => parameterNames.Contains(item.Key)).Select(item => string.Format("{0}={1}", item.Key, (item.Value == null) ? "" : item.ToString())));
            //this.Arguments = this.Arguments;
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

}
