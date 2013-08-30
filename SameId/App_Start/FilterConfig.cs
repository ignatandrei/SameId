using System.Web;
using System.Web.Mvc;
using SameIdAlert;

namespace SameId
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SameIdFilter());

        }
    }
}