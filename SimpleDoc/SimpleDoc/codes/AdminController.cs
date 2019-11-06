using System;
using System.Collections.Generic;
using System.Text;
using BeetleX.FastHttpApi;
using System.Linq;
namespace SimpleDoc.codes
{
    [Controller(BaseUrl = "admin")]
    [AdminFilter]
    public class AdminController
    {

        public AdminController()
        {

        }

        private SettingBase<DocumentManagement> Management = DocumentManagement.Default;
        [SkipFilter(typeof(AdminFilter))]
        public object Login(string name, string pwd, IHttpContext context)
        {
            if (name == Webconfig.Default.Data.Usename && pwd == Webconfig.Default.Data.Password)
            {
                JWTHelper.Default.CreateToken(context.Response, name, "admin");
                return true;
            }
            return false;
        }

        public object ListCategories()
        {
            return from a in Management.Data.Categories orderby a.Order ascending select a;
        }

        public object ListDocuments(string category)
        {
            var items = from a in Management.Data.Documents
                        where string.IsNullOrEmpty(category) || a.Category == category
                        select a;
            if (!string.IsNullOrEmpty(category))
                return from a in items orderby a.Order descending select a;
            return items;
        }


        public void EditCategory(Category body)
        {
            Management.Data.ModifyCategory(body.ID, body.Name, body.Remark, body.Order, body.Enabled);
            Management.Save();

        }



        public void EditDocument(Document body)
        {
            int count = Management.Data.Documents.Count(c => c.Category == body.Category);
            body.Order = count + 1;
            Management.Data.ModifyDocument(body.ID, body.Title, body.Category, body.Order, body.Enabled);
            Management.Save();
        }

        public void Save(string id, string content)
        {
            DocumentFile.Save(id, content);
            var item = Management.Data.Categories.FirstOrDefault(d => d.ID == id);
            if (item != null)
            {
                item.HasBody = true;
                Management.Save();
            }
        }

        public object GetDocument(string id, bool cate, IHttpContext context)
        {
            if (cate)
            {
                var result = Management.Data.Categories.FirstOrDefault(d => d.ID == id);
                return new { result?.ID, Title = result?.Name };
            }
            return Management.Data.Documents.FirstOrDefault(d => d.ID == id);
        }
        public void SaveDocumentContent(string id, string content, bool cate)
        {
            DocumentFile.Save(id, content);
            var item = Management.Data.Categories.FirstOrDefault(d => d.ID == id);
            if (item != null)
            {
                item.HasBody = true;
                Management.Save();
            }
        }

        public void SaveTitle(string title)
        {
            Webconfig.Default.Data.Title = title;
            Webconfig.Default.Save();
        }

        public void ReCreatKey()
        {
            Webconfig.Default.Data.CreateJWTKey();
            Webconfig.Default.Save();
            JWTHelper.Init();
        }

        public void ChangePwd(string pwd)
        {
            Webconfig.Default.Data.Password = pwd;
            Webconfig.Default.Save();
        }
        [SkipFilter(typeof(AdminFilter))]
        public object GetTitle()
        {
            return Webconfig.Default.Data.Title;
        }

        public object GetKey()
        {
            return Webconfig.Default.Data.JWTKey;
        }

        public object GetDocumentContent(string id)
        {
            return DocumentFile.Load(id);
        }

        public void RemoveCategory(string id)
        {
            Management.Data.RemoveCategory(id);
            Management.Save();
        }

        public void RemoveDocument(string id)
        {
            Management.Data.RemoveDocument(id);
            Management.Save();
            DocumentFile.Delete(id);
        }

        [Put]
        public string PasteImage(IHttpContext context)
        {
            string id = Guid.NewGuid().ToString("N");
            SaveFile(id + ".jpg", context);
            return id;
        }
        [Put]
        public string UploadFile(string name, IHttpContext context)
        {
            SaveFile(name, context);
            return name;
        }

        private void SaveFile(string file, IHttpContext context)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + $"views{System.IO.Path.DirectorySeparatorChar}files{System.IO.Path.DirectorySeparatorChar}";
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            string filename = path + file;
            var data = System.Buffers.ArrayPool<byte>.Shared.Rent(context.Request.Length);
            try
            {
                context.Request.Stream.Read(data, 0, data.Length);
                using (System.IO.FileStream stream = System.IO.File.Open(filename, System.IO.FileMode.Create))
                {
                    stream.Write(data, 0, context.Request.Length);
                    stream.Flush();
                }
            }
            finally
            {
                System.Buffers.ArrayPool<byte>.Shared.Return(data);
            }

        }
    }
}
