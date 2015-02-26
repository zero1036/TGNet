using System.Web;
using System.Web.Optimization;
/*****************************************************
* 目的：BundleConfig
* 创建人：
* 创建时间：
* 修改目的：统一存放格式
* 修改人：林子聪
* 修改时间：20141226
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region JQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js",
                       "~/Scripts/jquery.form.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            /* Jasonyiu 2014-11-28
            * 以上代码是原模型自带，将来修改样式后，可以删除。
            * 新使用的代码请写到注释下面。
            */
            bundles.Add(new ScriptBundle("~/Bundles/JQ").Include(
                      "~/Scripts/jquery-{version}.js"                           //JQ文件
                       ));


            bundles.Add(new ScriptBundle("~/Bundles/JS").Include(
                      "~/Scripts/Shared/Commom.js"                             //通用脚本
                       ));


            bundles.Add(new StyleBundle("~/Bundles/Css").Include(
                //"~/Content/site.css",                                         //系统样式，但现在系统还有自己的样式，用的是原项目样式
                "~/Content/Style.css"                                           //拟补site.css，自己加入的公用样式。
                ));


            bundles.Add(new StyleBundle("~/Bundles/Authority/Css").Include(
                "~/Content/authority/basic_layout.css",                         //原系统样式
                "~/Content/authority/common_style.css"                          //原系统样式
    ));
            #endregion

            #region Angular
            //Angular JS
            bundles.Add(new ScriptBundle("~/Bundles/angularjs").Include(
             "~/Scripts/Angular/angular.js",
             "~/Scripts/Angular/angular-route.js"));
            #endregion

            #region Bootstrap
            //Bootstrap JS
            bundles.Add(new ScriptBundle("~/Bundles/bootstrapjs").Include(
             "~/Scripts/Bootstrap/js/bootstrap.js"));
            //Bootstrap css
            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
             "~/Scripts/Bootstrap/css/bootstrap.css",
             "~/Scripts/Bootstrap/css/bootstrap.min.css"));
            #endregion

            #region EGCommon
            //EGCommon JS
            bundles.Add(new ScriptBundle("~/Bundles/egcommonjs").Include(
            "~/Scripts/EGCommon.js"));
            #endregion

            #region 开源插件
            #region Dropzone
            //Dropzone JS
            bundles.Add(new ScriptBundle("~/Bundles/dropzonejs").Include(
            "~/Scripts/Dropzone/dropzone.js"));
            //Dropzone css
            bundles.Add(new StyleBundle("~/Content/dropzonescss").Include(
            "~/Scripts/Dropzone/css/basic.css",
            "~/Scripts/Dropzone/css/dropzone.css"));
            #endregion

            #region zTree

            bundles.Add(new ScriptBundle("~/Bundles/ztreeJS").Include(
            "~/Scripts/zTree/jquery.ztree.all-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/zTreeStyle/ztreeCSS").Include(
            "~/Content/zTreeStyle/zTreeStyle.css"));

            #endregion

            #endregion

            #region Controllers
            //非常抱歉，Controllers JS是不能够被压缩的，
            //否则会出现未知错误
            #endregion

            //压缩打包
            BundleTable.EnableOptimizations = true;
        }
    }
}