using System.Web;
using System.Web.Optimization;

namespace KGS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region [Master css]

            bundles.Add(new StyleBundle("~/bundles/MasterLayoutCss").Include(
      
            ));

            #endregion

            #region [js Bundles]

            //Main Layout
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                  "~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.4.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/globalJs").Include("~/Scripts/alertify/alertify.min.js","~/Scripts/global.js"));
            bundles.Add(new ScriptBundle("~/bundles/validateJs").Include(
          "~/Scripts/jquery.validate.min.js",
          "~/Scripts/jquery.validate.unobtrusive.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/chartjs").Include(
        "~/Scripts/chart.js/dist/Chart.min.js", "~/js/off-canvas.js", "~/js/hoverable-collapse.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
        "~/Scripts/jquery.dataTables.js",
        "~/Scripts/dataTables.bootstrap4.js", "~/Scripts/jquery.fnSetFilteringEnterPress.js"
        ));
            #endregion
            #region [Login Layout]

            bundles.Add(new StyleBundle("~/bundles/LoginLayoutCss").Include(
               "~/MainPage/fonts/font-awesome.css",
               "~/MainPage/perfect-scrollbar.css",
               "~/MainPage/loginstyle.css", "~/Content/alertify/alertify.min.css"
              ));
            bundles.Add(new ScriptBundle("~/bundles/LoginLayoutjs").Include(
         "~/Scripts/jquery.js",
          "~/Scripts/popper.min.js",
         "~/Scripts/alertify/bootstrap.min.js",
         "~/Scripts/perfect-scrollbar.jquery.min.js",
         "~/js/misc.js"
        ));
        

            BundleTable.EnableOptimizations = true;

            #endregion
        }
    }
}
