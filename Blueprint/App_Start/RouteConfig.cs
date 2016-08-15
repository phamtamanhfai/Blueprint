using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blueprint
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home-Test",
                url: "Bai-kiem-tra",
                defaults: new { controller = "Home", action = "Test"}
            );

            routes.MapRoute(
                name: "Home-DoTest",
                url: "Bai-kiem-tra/Bai-{id}",
                defaults: new { controller = "Home", action = "DoTest", id = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "Home-About",
                url: "Gioi-thieu",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "Home-Feedback",
                url: "Chia-se-hoc-vien",
                defaults: new { controller = "Home", action = "Feedback" }
            );

            routes.MapRoute(
                name: "Home-Enquire",
                url: "Dang-ky",
                defaults: new { controller = "Home", action = "Enquire"}
            );

            routes.MapRoute(
                name: "Home-Course",
                url: "Khoa-hoc/{title}",
                defaults: new { controller = "Home", action = "Course", title = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Home-Fees",
                url: "Hoc-phi",
                defaults: new { controller = "Home", action = "Fees" }
            );

            routes.MapRoute(
                name: "Admin-Login",
                url: "Dang-nhap",
                defaults: new { controller = "Admin", action = "Login" }
            );

            routes.MapRoute(
                name: "Admin-Logout",
                url: "Dang-xuat",
                defaults: new { controller = "Admin", action = "Logout" }
            );

            routes.MapRoute(
                name: "Admin-Pass",
                url: "Doi-mat-khau",
                defaults: new { controller = "Admin", action = "ChangePass" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
