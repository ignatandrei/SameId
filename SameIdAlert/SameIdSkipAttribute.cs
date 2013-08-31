using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SameIdAlert
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SameIdSkipAttribute : Attribute
    {

    }
}