using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace SameIdAlert
{
    public class SameIdAssemblyLocator: IAssemblyLocator
    {
        public IList<Assembly> GetAssemblies()
        {
            return new List<Assembly>() { Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly() };
        }
    }
}
