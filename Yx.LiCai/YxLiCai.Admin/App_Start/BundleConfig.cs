using System.Web;
using System.Web.Optimization;

namespace YxLiCai.Admin
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css")
                .Include("~/Content/*.css"));

            bundles.Add(new ScriptBundle("~/js")
                           .Include("~/Scripts/bootbox.js")
                           .Include("~/Scripts/bootstrap-timepicker.js")
                           .Include("~/Scripts/jquery.validate.js")
                           .Include("~/Scripts/Extension/*.js")
                           .Include("~/Scripts/jquery.loadmask.js"));
        }
    }
}