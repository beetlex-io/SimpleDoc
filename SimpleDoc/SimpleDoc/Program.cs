using BeetleX.FastHttpApi.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace SimpleDoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {                    
                    services.UseBeetlexHttp(o =>
                    {
                        
                        o.BufferPoolMaxMemory = 100;
                        o.SetDebug();
                        o.LogLevel = BeetleX.EventArgs.LogType.Warring;
                        codes.JWTHelper.Init();
                    },
                    c =>
                    {
                        codes.CodeStyles.Load();
                        c.UrlRewrite.Add("/admin/doc/{1}.html", "/admin/edit.html");
                    },
                    typeof(Program).Assembly);
                });
            builder.Build().Run();
        }
    }
}
