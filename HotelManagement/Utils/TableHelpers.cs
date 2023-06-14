using HotelManagement.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagement.Utils
{
    public static class TableHelpers
    {
        public static IHtmlContent GetSortArrow(TableOptions options, IUrlHelper urlHelper)
        {
            string arrow;
            string? order;
            if (options.Sort == options.Column)
            {
                switch (options.Order)
                {
                    case "asc":
                        arrow = "⇑";
                        order = "desc";
                        break;
                    case "desc":
                        arrow = "⇓";
                        order = null;
                        break;
                    default:
                        arrow = "⇅";
                        order = "asc";
                        break;
                }
            }
            else
            {
                arrow = "⇅";
                order = "asc";
            }

            var tagBuilder = new TagBuilder("a");
            tagBuilder.InnerHtml.Append(arrow);
            tagBuilder.AddCssClass("sort-arrow");
            tagBuilder.Attributes.Add("href",
                urlHelper.Action(options.ActionName, options.ControllerName,
                    new
                    {
                        keyword = options.Keyword, sort = options.Column, order,
                        page = options.Page, size = options.Size
                    }));

            return tagBuilder;
        }
    }
}