using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SimpleDoc.codes
{
    class DocumentManagement
    {

        public List<Category> Categories { get; set; } = new List<Category>();

        public List<Document> Documents { get; set; } = new List<Document>();

        public void ModifyCategory(string id, string name, string remark, int order, bool enabled)
        {
            if (order == 0)
                order = Categories.Count;
            var item = Categories.FirstOrDefault(p => p.ID == id);
            if (item == null)
            {
                item = new Category();
                item.ID = Guid.NewGuid().ToString("N");
                item.Order = Categories.Count;
                item.Name = name;
                item.Order = order;
                item.Remark = remark;
                item.Enabled = enabled;
                Categories.Add(item);
            }
            else
            {
                item.Enabled = enabled;
                item.Order = order;
                item.Name = name;
                item.Remark = remark;
            }
        }

        public void RemoveCategory(string id)
        {
            Categories.RemoveAll(p => p.ID == id);
        }

        public Document ModifyDocument(string id, string title, string category, int order, bool enabled)
        {
            var item = Documents.FirstOrDefault(p => p.ID == id);
            if (item == null)
            {
                item = new Document();
                item.ID = Guid.NewGuid().ToString("N");
                item.Order = order;
                item.Category = category;
                item.Title = title;
                item.Enabled = enabled;
                Documents.Add(item);
            }
            else
            {
                item.Category = category;
                item.Title = title;
                item.Order = order;
                item.Enabled = enabled;
            }
            return item;
        }

        public void RemoveDocument(string id)
        {
            Documents.RemoveAll(p => p.ID == id);
        }

        private static SettingBase<DocumentManagement> mDefault;

        public static SettingBase<DocumentManagement> Default
        {
            get
            {
                if (mDefault == null)
                {
                    mDefault = new SettingBase<DocumentManagement>("document.json");
                    mDefault.Load();
                }
                return mDefault;
            }
        }

    }
    public class Category
    {

        public Category()
        {

        }

        public bool HasBody { get; set; }

        public int Order { get; set; }

        public string ID { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }
    }

    public class Document
    {
        public Document()
        {

        }

        public int Order { get; set; }

        public string ID { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public bool Enabled { get; set; }
    }

    class DocumentFile
    {
        static DocumentFile()
        {
            Path = AppDomain.CurrentDomain.BaseDirectory + $"views{System.IO.Path.DirectorySeparatorChar}document{System.IO.Path.DirectorySeparatorChar}";
            if (!System.IO.Directory.Exists(Path))
                System.IO.Directory.CreateDirectory(Path);
        }

        static string Path;

        public static void Delete(string id)
        {
            string file = Path + id + ".md";
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);
        }

        public static void Save(string id, string content)
        {
            string file = Path + id + ".md";
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(file, false, Encoding.UTF8))
            {
                writer.Write(content);
                writer.Flush();
            }
        }

        public static string Load(string id)
        {
            string file = Path + id + ".md";
            if (System.IO.File.Exists(file))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(file, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            return String.Empty;
        }
    }

}
