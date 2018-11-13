using System;

namespace SameIDAlert
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SameIdSkipAttribute:Attribute
    {
    }

}
