using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc
{
    /// <summary>
    /// MvcHtmlString扩展方法
    /// </summary>
    public static class MvcHtmlStringExtension
    {

        #region 显示Model的Description

        /// <summary>
        /// 显示Model的Description(会用span的形式)
        /// </summary>
        /// <typeparam name="TModel">Model类型</typeparam>
        /// <typeparam name="TValue">Model</typeparam>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">表达式，写明字段</param>
        /// <param name="htmlAttributes">html附加属性</param>
        /// <returns></returns>
        public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            //获取描述
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string description = metadata.Description;

            //转化为html属性
            RouteValueDictionary anonymousObjectToHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            //生成html标签
            TagBuilder tagBuilder = new TagBuilder("span");
            tagBuilder.MergeAttributes<string, object>(anonymousObjectToHtmlAttributes);
            tagBuilder.SetInnerText(description);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        #endregion

    }
}
