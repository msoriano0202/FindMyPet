using System.Web;
using System.Web.Optimization;

namespace FindMyPet.MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // Bundles for CNoteTheme
            bundles.Add(new StyleBundle("~/bundles/theme-vendor-css").Include(
                       "~/CNoteV1.0/assets/vendor/css/font-awesome.css",
                       "~/CNoteV1.0/assets/vendor/css/simple-line-icons.css",
                       "~/CNoteV1.0/assets/vendor/css/owl.carousel.css",
                       "~/CNoteV1.0/assets/vendor/css/magnific-popup.css",
                       "~/CNoteV1.0/assets/vendor/css/jquery-ui-slider.css"));

            bundles.Add(new StyleBundle("~/bundles/theme-assets-css").Include(
                      "~/CNoteV1.0/cnote-main/assets/css/main.css",
                      "~/CNoteV1.0/cnote-main/assets/css/custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/theme-vendor-js").Include(
                     "~/CNoteV1.0/assets/vendor/js/jquery.js",
                     "~/CNoteV1.0/assets/vendor/js/popper.js",
                     "~/CNoteV1.0/assets/vendor/js/bootstrap.js",
                     "~/CNoteV1.0/assets/vendor/js/owl.carousel.js",
                     "~/CNoteV1.0/assets/vendor/js/owl.carousel2.thumbs.js",
                     "~/CNoteV1.0/assets/vendor/js/jquery.appear.js",
                     "~/CNoteV1.0/assets/vendor/js/jquery.countdown.js",
                     "~/CNoteV1.0/assets/vendor/js/countUp.js",
                     "~/CNoteV1.0/assets/vendor/js/masonry.pkgd.js",
                     "~/CNoteV1.0/assets/vendor/js/imagesloaded.pkgd.js",
                     "~/CNoteV1.0/assets/vendor/js/jquery.magnific-popup.js",
                     "~/CNoteV1.0/assets/vendor/js/circle-progress.js",
                     "~/CNoteV1.0/assets/vendor/js/typed.js",
                     "~/CNoteV1.0/assets/vendor/js/scrollreveal.js",
                     "~/CNoteV1.0/assets/vendor/js/widget.js",
                     "~/CNoteV1.0/assets/vendor/js/mouse.js",
                     "~/CNoteV1.0/assets/vendor/js/slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/theme-assets-js").Include(
                    "~/CNoteV1.0/cnote-main/assets/js/cnote.js"));


            bundles.Add(new ScriptBundle("~/bundles/bootbox-js").Include(
                               "~/Scripts/bootbox.min.js"));
        }
    }
}
