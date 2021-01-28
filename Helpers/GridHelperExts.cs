using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcJqGrid;
using MvcJqGrid.Enums;

namespace FCInformesSolucion.Helpers
{
    public static class GridHelperExts
    {
        public static Grid MyjqGrid(this HtmlHelper htmlHelper, string id, int pageSize, string onGridComplete = "", string onSelectRow = "")
        {
            var obj = new Grid(id)
                .OnSelectRow(onSelectRow)
                .SetRequestType(RequestType.Get)
                .SetRowNum(pageSize)
                .SetSortOrder(SortOrder.Desc)
                .SetShrinkToFit(true)
                .SetAutoWidth(true)
                .SetShowAllSortIcons(true)
                .SetViewRecords(true)
                .SetLoadText("...")
                .SetEmptyRecords("No Existen registros")
                .OnLoadComplete("updateGridInfo(this)")
                .SetAltRows(true).SetAltClass("altClass");

            if (!String.IsNullOrEmpty(onGridComplete))
                obj.OnGridComplete(onGridComplete);
            return obj;
        }

        #region Column methods

        public static Column Column(string columnName, object columnProperties = null)
        {
            var column = new Column(columnName).SetHidden(true);
            ApplyProperties(column, columnProperties);
            return column;
        }

        public static Column Column(string columnName, int width, string columnLabel, object columnProperties = null)
        {
            var column = new Column(columnName).SetWidth(width).SetLabel(columnLabel);
            ApplyProperties(column, columnProperties);
            return column;
        }

        private static void ApplyProperties(Column column, object columnProperties)
        {
            if (columnProperties == null) return;
            var columnType = column.GetType();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(columnProperties))
            {
                var name = "set" + propertyDescriptor.Name;
                var methodInfo = columnType.GetMethod(name, new[] { propertyDescriptor.PropertyType });
                if (methodInfo == null) throw new InvalidOperationException("Invalid method: " + name + ".");
                methodInfo.Invoke(column, new[] { propertyDescriptor.GetValue(columnProperties) });
            }
        }

        #endregion

        #region Grid methods

        public static Grid Column(this Grid grid, string columnName, object columnProperties = null)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            grid.AddColumn(Column(columnName, columnProperties));
            return grid;
        }

        public static Grid Column(this Grid grid, string columnName, int width, string columnLabel, object columnProperties = null)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            grid.AddColumn(Column(columnName, width, columnLabel, columnProperties));
            return grid;
        }

        #endregion

        #region Pagination

        public static MvcHtmlString PaginationBtn(this HtmlHelper html, string grid, string @class = "")
        {

            var stringHtml = string.Format(@"<ul class='pagination {1}'>
                                                     <li  value='' class='btnHome' grid='{0}' ><span>  <i class='fa fa-fast-backward fa-lg'></i></span> </li>
                                                     <li  value='' class='btnPrev' grid='{0}'><span>  <i class='fa fa-backward fa-lg'></i></span> </li>
                                                     <li class='' ><span class='lblInfo' grid='{0}'></span></li>
                                                     <li  value='' class='btnNext' grid='{0}'><span>  <i class='fa fa-forward fa-lg'></i></span> </li>
                                                     <li  value='' class='btnEnd' grid='{0}'><span>  <i class='fa fa-fast-forward fa-lg'></i></span> </li>
                                               </ul>", grid, @class);

            return new MvcHtmlString(stringHtml);
        }

        #endregion

        #region Actions

        public static TagBuilder ActionsList(string modalId, string @class = null)
        {
            var ul = new TagBuilder("ul");
            ul.AddCssClass("list-inline actions-list");
            ul.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new
            {
                @class = @class ?? string.Empty,
                data_modal = modalId,
            }));

            return ul;
        }

        public static TagBuilder AddRange(this TagBuilder tag, IEnumerable<IHtmlString> actions)
        {
            foreach (var actionHtml in actions)
            {
                var tagHtml = new StringBuilder(tag.InnerHtml);
                tagHtml.Append(actionHtml);

                tag.InnerHtml = tagHtml.ToString();
            }
            return tag;
        }

        public static TagBuilder Add(this TagBuilder tag, IHtmlString actionHtml)
        {
            var tagHtml = new StringBuilder(tag.InnerHtml);
            tagHtml.Append(actionHtml);

            tag.InnerHtml = tagHtml.ToString();
            return tag;
        }

        public static IHtmlString EditAction(string url, object id = null, string callback = null,
            string hiddenId = "Id", string @class = null, bool viewFromServerUrl = false)
        {
            id = id ?? "";
            var edit = new TagBuilder("li");
            edit.AddCssClass(@class);

            var editLnk = new TagBuilder("a");
            editLnk.AddCssClass("maintenance-edit btn btn-primary btn-xs");
            editLnk.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new
            {
                data_action = url,
                data_id = id.ToString(),
                data_callback = callback,
                data_hidden = hiddenId,

                // tooltip
                data_toggle = "tooltip",
                title = "Edit",
            }));

            if (viewFromServerUrl)
            {
                editLnk.MergeAttribute("data-viewurl", "true");
            }

            editLnk.InnerHtml = "<i class='glyphicon glyphicon-pencil'></i>";

            edit.InnerHtml = editLnk.ToString();
            return MvcHtmlString.Create(edit.ToString());
        }

        public static IHtmlString ViewAction(string url, object id = null, string callback = null,
            string hiddenId = "Id", string @class = null, bool viewFromServerUrl = false)
        {
            id = id ?? "";
            var edit = new TagBuilder("li");
            edit.AddCssClass(@class);

            var editLnk = new TagBuilder("a");
            editLnk.AddCssClass("maintenance-edit btn btn-primary btn-xs");
            editLnk.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new
            {
                data_action = url,
                data_id = id.ToString(),
                data_callback = callback,
                data_hidden = hiddenId,

                // tooltip
                data_toggle = "tooltip",
                title = "Ver",
            }));

            if (viewFromServerUrl)
            {
                editLnk.MergeAttribute("data-viewurl", "true");
            }

            editLnk.InnerHtml = "<i class='glyphicon glyphicon-eye-open'></i>";

            edit.InnerHtml = editLnk.ToString();
            return MvcHtmlString.Create(edit.ToString());
        }

        public static IHtmlString DeleteAction(string url, string gridId, object id = null, string @class = null)
        {
            id = id ?? "";
            var delete = new TagBuilder("li");
            delete.AddCssClass(@class);

            var deleteLnk = new TagBuilder("a");
            deleteLnk.AddCssClass("maintenance-delete");
            deleteLnk.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new
            {
                data_action = url,
                data_grid = gridId,
                data_id = id,

                // tooltips
                data_toggle = "tooltip",
                title = "Delete",
            }));
            deleteLnk.InnerHtml = "<i class='delete-action glyphicon glyphicon-trash'></i>";

            delete.InnerHtml = deleteLnk.ToString();
            return MvcHtmlString.Create(delete.ToString());
        }

        public static IHtmlString End(this TagBuilder tag)
        {
            return MvcHtmlString.Create(tag.ToString());
        }

        public static IHtmlString CreateLink(string url, string text, string callback = null, string title = "", string @class = null)
        {
            var link = new TagBuilder("a");
            if (@class != null)
                link.AddCssClass(@class);
            link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new
            {
                href = url,
                data_callback = callback,
                // tooltip
                title,
            }));
            link.InnerHtml = text;
            return MvcHtmlString.Create(link.ToString());
        }

        public static IHtmlString CreateFontAweson(string fontType, string extraClass, string title = "")
        {
            var i = new TagBuilder("i");
            i.AddCssClass(string.Format("fa {0}", fontType));

            if (extraClass != null)
            {
                i.AddCssClass(extraClass);
            }

            i.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new { title }));
            return MvcHtmlString.Create(i.ToString());
        }

        #endregion
    }
}