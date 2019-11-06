using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDoc.codes
{
    public class SettingBase<T> where T : new()
    {
        public SettingBase(string file)
        {
            mFile = file;
        }

        private string mFile;

        public T Data { get; set; } = new T();

        public void Load()
        {
            if (System.IO.File.Exists(mFile))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(mFile, Encoding.UTF8))
                {
                    string value = reader.ReadToEnd();
                    Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
                }
            }
        }

        public void Save()
        {
            using (System.IO.StreamWriter write = new System.IO.StreamWriter(mFile, false, Encoding.UTF8))
            {
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(Data);
                write.Write(data);
                write.Flush();
            }
        }

    }
}
