using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blueprint.Utils
{
    public class CourseFilterAttribute : ActionFilterAttribute
    {
        
        public static IDictionary<string, int> CourseTitles = new Dictionary<string, int>();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var title = filterContext.RouteData.Values["title"] as string;
            if (title != null)
            {
                int id;
                CourseTitles.TryGetValue(title, out id);
                filterContext.ActionParameters["id"] = id;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}