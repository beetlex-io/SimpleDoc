using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SimpleDoc.codes
{
    [BeetleX.FastHttpApi.Controller]
    public class HomeController
    {

        private SettingBase<DocumentManagement> Management = DocumentManagement.Default;

        public object List()
        {
            var categories = from a in Management.Data.Categories
                             where a.Enabled
                             orderby a.Order ascending
                             select new
                             {
                                 a.ID,
                                 Expand = false,
                                 a.HasBody,
                                 IsCategory = true,
                                 a.Name,
                                 Selected = false,
                                 a.Remark,
                                 Items = from d in Management.Data.Documents
                                         where d.Enabled && d.Category == a.ID
                                         orderby d.Order ascending
                                         select new { d.ID, d.Title, d.Category, Selected = false }
                             };
            return categories;
        }

        public object GetContent(string id)
        {
            return DocumentFile.Load(id);
        }

        public void SelectCodeStyle(string style, BeetleX.FastHttpApi.IHttpContext context)
        {
            context.Response.SetCookie("CodeStyle", style);
        }

        public object GetStyle(BeetleX.FastHttpApi.IHttpContext context)
        {
            string style = context.Request.Cookies["CodeStyle"];
            if (style == null)
                style = "railscasts";
            return CodeStyles.GetCss(style);
        }

        public object WebsiteInfo(BeetleX.FastHttpApi.IHttpContext context)
        {
            string style = context.Request.Cookies["CodeStyle"];
            if (style == null)
                style = "railscasts";
            return new
            {
                Webconfig.Default.Data.Title,
                CodeStyles = CodeStyles.GetNames(),
                SelectStyle = style
            };
        }
    }
}
