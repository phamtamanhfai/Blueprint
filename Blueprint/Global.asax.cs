using Blueprint.DbOperation;
using Blueprint.Models;
using Blueprint.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Blueprint
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var db = new BaseDA("YourCSName")) {
                var listCourse = db.GetListCourses();
                RegisterRouteCourse(CourseFilterAttribute.CourseTitles, listCourse);
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var usr = HttpContext.Current.User;

            if (usr != null && usr.Identity.IsAuthenticated && usr.Identity is FormsIdentity)
            {
                HttpContext.Current.User = new UserPrincipal(usr.Identity);
            }
        }

        public void RegisterRouteCourse(IDictionary<string,int> dict, List<CoursesItem> list) {

            foreach (var item in list)
            {
                string route = Utility.EnterByDash(Utility.ConvertToUnsign(item.Title));
                dict.Add(route, item.ID);
            }
        }

        
    }
}
