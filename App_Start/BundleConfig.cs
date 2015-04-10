using System.Web;
using System.Web.Optimization;

namespace HelpDesk
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js",
            //            "~/Scripts/login.js",
            //            "~/Scripts/jquery.signalR-2.2.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTable").Include(
                        "~/Content/jquery.dataTables.min.js",
                        "~/Content/dataTables.bootstrap.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //            "~/Scripts/bootstrap.min.js"
            //            ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/newTickets").Include(
                        "~/Scripts/newTicket.js"));

            bundles.Add(new ScriptBundle("~/bundles/graphs").Include(
                        "~/bower_components/morrisjs/morris.min.js",
                        "~/js/morris-data.js"));


            bundles.Add(new ScriptBundle("~/bundles/TemplateJS").Include(
                        "~/bower_components/jquery/dist/jquery.min.js",
                        "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                        "~/bower_components/metisMenu/dist/metisMenu.min.js",
                        "~/bower_components/raphael/raphael-min.js",
                        "~/dist/js/sb-admin-2.js",
                        "~/Scripts/login.js"
                        ));


            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"
            //            ));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"
            //        , "~/Content/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/TemplateCSS").Include(
                        "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                        "~/bower_components/metisMenu/dist/metisMenu.min.css",
                        "~/dist/css/timeline.css",
                        "~/dist/css/sb-admin-2.css",
                        "~/bower_components/morrisjs/morris.css",
                        "~/bower_components/font-awesome/css/font-awesome.min.css"
                        ));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css",
            //            "~/Content/jquery.dataTables.css"));
        }
    }
}