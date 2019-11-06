using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDoc.codes
{
    public class Webconfig
    {
        public Webconfig()
        {
            Usename = "admin";
            Password = "123456";
            Title = "Simple doc";
            CreateJWTKey();
        }

        public string Title { get; set; }

        public string Usename { get; set; }

        public string Password { get; set; }

        public byte[] JWTKey { get; set; }

        public void CreateJWTKey()
        {
            byte[] key = new byte[128];
            new Random().NextBytes(key);
            JWTKey = key;
        }

        private static SettingBase<Webconfig> mDefault;
        public static SettingBase<Webconfig> Default
        {
            get
            {
                if (mDefault == null)
                {
                    mDefault = new SettingBase<Webconfig>("webconfig.json");
                    mDefault.Load();
                }
                return mDefault;
            }

        }

    }
}
