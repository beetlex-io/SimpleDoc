using BeetleX.Buffers;
using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleDoc.codes
{
    public class CodeStyles
    {

      

        private static Dictionary<string, Item> mCssTable = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);

        public static void Load()
        {
            var assembly = typeof(CodeStyles).Assembly;
            string[] files = assembly.GetManifestResourceNames();
            foreach (var file in files)
            {
                if (file.IndexOf(".views.styles") >= 0)
                {
                    string style = System.IO.Path.GetFileNameWithoutExtension(file);
                    using (System.IO.Stream stream = assembly.GetManifestResourceStream(file))
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                        {
                            int start = style.LastIndexOf('.');
                            Item item = new Item();
                            item.Css = reader.ReadToEnd();
                            item.Name = style.Substring(start+1);
                            
                            mCssTable[item.Name] = item;
                        }
                    }
                }
            }
        }

        public static string[] GetNames()
        {
            return mCssTable.Keys.ToArray();
        }

        public static CssTextResult GetCss(string name)
        {
            if(mCssTable.TryGetValue(name,out Item result))
            {
                return new CssTextResult(result.Css);
            }
            return new CssTextResult("");
        }


        public class CssTextResult : ResultBase
        {
            public CssTextResult(string text)
            {
                Text = text == null ? "" : text;
            }

            public override IHeaderItem ContentType => new HeaderItem("Content-Type: text/css");

            public string Text { get; set; }

            public override bool HasBody => true;

            public override void Write(PipeStream stream, HttpResponse response)
            {
                stream.Write(Text);
            }
        }


        public class Item
        {
            public string Name { get; set; }

            public string Css { get; set; }
        }

    }
}
