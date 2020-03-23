using System.Web;
using System.Web.Optimization;

namespace CMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/mainjs").Include(
             "~/Content/js/plugin/pace/pace.min.js",
             "~/Content/js/libs/jquery-2.1.1.min.js",
              "~/Content/js/libs/jquery-ui-1.10.3.min.js",
               "~/Content/js/app.config.js",
                "~/Content/js/bootstrap/bootstrap.min.js",
                 "~/Content/js/plugin/jquery-validate/jquery.validate.min.js",
                  "~/Content/js/plugin/masked-input/jquery.maskedinput.min.js",
             "~/Content/js/app.min.js"
             ));
            bundles.Add(new ScriptBundle("~/bundles/formlogin").Include(
                "~/Content/js/libs/jquery-2.1.1.min.js",
                "~/Content/js/libs/jquery-ui-1.10.3.min.js",
                "~/Content/js/bootstrap/bootstrap.min.js",
                 "~/Content/js/plugin/jquery-validate/jquery.validate.min.js",
                  "~/Content/js/plugin/masked-input/jquery.maskedinput.min.js",
                   "~/Content/js/app.min.js"

                ));
        }
    }
}
