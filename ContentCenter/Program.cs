using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using ContentCenter.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace ContentCenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Environment.OSVersion.Platform == PlatformID.Win32NT
            var _advenced = GetSettingAdvenced();
            UriBuilder uri = new UriBuilder(new Uri(
                    $"{_advenced.Scheme}://{_advenced.Host}:{_advenced.Port}"));

            if (uri.Scheme.ToLower() == "http")
            {
                BuildWebHost(args, uri.Uri.OriginalString).Run();
            }
            else if (uri.Scheme.ToLower() == "https")
            {
                var certificate = GetCetrtificate();
                BuildWebHostSSL(args, certificate.Path, certificate.Key, uri.Uri.OriginalString).Run();
            }
            else
            {
                Log.Error("Service not started");
                return;
            }               
        }

        public static IWebHost BuildWebHost(string[] args, string uri) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()    
                .UseUrls(uri)               
                .Build();

        public static IWebHost BuildWebHostSSL(string[] args, string pathForCertificatre, string password, string uri) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(cfg => cfg.Listen(IPAddress.Parse("127.0.0.1"),
                    5003,
                    opt => opt.UseHttps(new X509Certificate2(pathForCertificatre, password))))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    IHostingEnvironment env = builderContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls(uri)
                .Build();


        #region private methods

        private static Settings GetSettings()
        {
            Settings settings = JsonConvert.DeserializeObject<Settings>(
                File.ReadAllText(Directory.GetCurrentDirectory() + "//appsettings.json"));
            return settings;
        }
        private static Certificate GetCetrtificate()
        {
            Settings settings = GetSettings();
            return settings.Logging.Certificate;
        }
        private static Advenced GetSettingAdvenced()
        {
            Settings settings = GetSettings();

            return settings.Logging.Advenced;
        }
        #endregion        
    }
}
