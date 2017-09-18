using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

public static class CheckRequiredLabelForHelper
{
    /// <summary>
    /// Appends a star if the field has the required DataAnnotation
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="html"></param>
    /// <param name="expression"></param>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public static MvcHtmlString CheckRequiredLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes)
    {
        var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        var resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName;
        if (metadata.IsRequired)
        {
            resolvedLabelText += "*";
        }
        return html.LabelFor(expression, resolvedLabelText, htmlAttributes);
    }
}
