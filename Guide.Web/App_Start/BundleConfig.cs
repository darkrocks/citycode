using System.Web;
using System.Web.Optimization;

namespace Guide.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery-edge").Include(
			"~/Scripts/jquery-2.1.0.min.js"));
			bundles.Add(new ScriptBundle("~/bundles/jquery-171").Include(
						"~/Scripts/jquery-1.7.1.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*",
						"~/Scripts/jquery.bootstrap3.validate.js",
						"~/Scripts/jquery.validate.unobtrusive.min.js"
			));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"
					  ));

			bundles.Add(new ScriptBundle("~/bundles/thirdparty").Include(
					  "~/Scripts/jquery.blueimp-gallery.min.js",
					  "~/Scripts/bootstrap-image-gallery.min.js",
					  "~/Scripts/jquery.cookie.js"));

			bundles.Add(new ScriptBundle("~/bundles/guide").Include(
				"~/Scripts/app/guide.js",
					  "~/Scripts/app/*.js"));

			bundles.Add(new ScriptBundle("~/bundles/textEditor").Include(
				"~/Scripts/ckeditor/ckeditor.js"));

			bundles.Add(new StyleBundle("~/Content/textEditor").Include(
							"~/Scripts/ckeditor/*.css"));

			bundles.Add(new StyleBundle("~/Content/css-layout").Include(
				"~/Content/Css/Bootstrap/bootstrap.css",
				"~/Content/Css/Bootswatch/bootswatch.css",
				"~/Content/font-awesome/css//font-awesome.min.css"));

			bundles.Add(new StyleBundle("~/Content/app").Include(
	"~/Content/Css/Bootrstrap-Image-Gallery/blueimp-gallery.min.css",
	"~/Content/Css/Bootrstrap-Image-Gallery/bootstrap-image-gallery.min.css",
	"~/Content/Css/world-flags-sprite-master/stylesheets/flags32.css",
	"~/Content/Css/app.css"));

			bundles.Add(new StyleBundle("~/Content/css-mobile").Include(
				"~/Content/Css/app-mobile.css"));

			bundles.Add(new StyleBundle("~/Content/css-desktop").Include(
				"~/Content/Css/app-desktop.css"));

			bundles.Add(new StyleBundle("~/Content/homepage").Include(
					"~/Content/Css/homepage.css",
					"~/Content/Css/world-flags-sprite-master/stylesheets/flags32.css"));
		}
	}
}
