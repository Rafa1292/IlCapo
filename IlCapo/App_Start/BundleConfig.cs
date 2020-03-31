using System.Web;
using System.Web.Optimization;

namespace IlCapo
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            //--------------------------------Personal scripts-----------------------------------------//

            //layout scripts//
            bundles.Add(new ScriptBundle("~/bundles/LayoutScripts").Include(
            "~/Scripts/PersonalScripts/router.js",
            "~/Scripts/PersonalScripts/loader.js",
            "~/Scripts/PersonalScripts/animations.js"
            ));



            //index scripts//
            bundles.Add(new ScriptBundle("~/bundles/IndexScripts").Include(
            "~/Scripts/PersonalScripts/search.js",
            "~/Scripts/PersonalScripts/bill.js",
            "~/Scripts/PersonalScripts/pays.js",
            "~/Scripts/PersonalScripts/ToGo.js"

            ));



            //---------------------------------------------------------------------------------------------//

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
