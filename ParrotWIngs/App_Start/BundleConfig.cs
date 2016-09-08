using System.Web;
using System.Web.Optimization;

namespace ParrotWIngs
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.11.1.min.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/angular-material.min.css",
                      "~/Content/font-awesome-4.6.3/css/font-awesome.min.css",
                      "~/Content/jquery.mCustomScrollbar.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-animate.min.js",
                "~/Scripts/angular-aria.min.js",
                "~/Scripts/angular-material.min.js",
                "~/Scripts/angular-messages.min.js",
                "~/Scripts/common.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/mCustomScrollbar").Include(
                "~/Scripts/jquery.mCustomScrollbar.concat.min.js"
                ));
        }
    }
}
