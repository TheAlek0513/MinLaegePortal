using System.Web;
using System.Web.Mvc;

namespace MinLægePortalMVC_custom
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
