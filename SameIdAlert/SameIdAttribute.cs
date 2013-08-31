using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SameIdAlert
{
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
}