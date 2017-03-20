using GuamApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ManagementApplication
{
    public static class HtmlHelper
    {
        public static MvcHtmlString CheckBoxTooltipFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression)
        {
            ModelMetadata modelMetadatum = ModelMetadata.FromLambdaExpression<TModel, bool>(expression, htmlHelper.ViewData);
            MvcHtmlString mvcHtmlString = InputExtensions.CheckBoxFor<TModel>(htmlHelper, expression, new { Title = modelMetadatum.Description, data_toggle = "tooltip" });
            return mvcHtmlString;
        }

        
        public static MvcHtmlString EditorTooltipFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
            return html.TextBoxTooltipFor<TModel, TValue>(expression);
        }
        

        public static MvcHtmlString TextAreaTooltipFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            ModelMetadata modelMetadatum = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
            MvcHtmlString mvcHtmlString = TextAreaExtensions.TextAreaFor<TModel, TValue>(html, expression, new { Title = modelMetadatum.Description, data_toggle = "tooltip", rows = "5" });
            return mvcHtmlString;
        }

        
        public static MvcHtmlString TextBoxTooltipFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.TextBoxTooltipFor<TModel, TValue>(expression, null);
        }
         

        public static MvcHtmlString TextBoxTooltipFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object options)
        {
            MvcHtmlString mvcHtmlString;
            ModelMetadata modelMetadatum = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
            SelectList selectList = options as SelectList;
            if (selectList == null)
            {
                IEnumerable<Attribute> item = modelMetadatum.AdditionalValues["Attributes"] as IEnumerable<Attribute>;
                if (item != null)
                {
                    Options option = item.OfType<Options>().FirstOrDefault<Options>();
                    RangeAttribute rangeAttribute = item.OfType<RangeAttribute>().FirstOrDefault<RangeAttribute>();
                    if (option != null)
                    {
                        selectList = new SelectList(option.Items);
                    }
                    else if (rangeAttribute != null)
                    {
                        int? minimum = (int?)(rangeAttribute.Minimum as int?);
                        int? maximum = (int?)(rangeAttribute.Maximum as int?);
                        (
                            from d in item.OfType<DefaultValueAttribute>()
                            select (int?)(d.Value as int?)).FirstOrDefault<int?>();
                        if ((!minimum.HasValue ? false : maximum.HasValue))
                        {
                            List<int> nums = new List<int>();
                            for (int i = minimum.Value; i < maximum.Value; i++)
                            {
                                nums.Add(i);
                            }
                            selectList = new SelectList(nums, (object)20);
                        }
                    }
                }
            }
            mvcHtmlString = (selectList == null ? InputExtensions.TextBoxFor<TModel, TValue>(html, expression, new { Title = modelMetadatum.Description, data_toggle = "tooltip", style = string.Concat("width:", (modelMetadatum.ModelType != typeof(string) ? "2em" : "")) }) : SelectExtensions.DropDownListFor<TModel, TValue>(html, expression, selectList, new { Title = modelMetadatum.Description, data_toggle = "tooltip" }));
            return mvcHtmlString;
        }
    }
}