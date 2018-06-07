using ContentCenter.Models;
using ContentCenter.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContentCenter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .Enrich.FromLogContext()
                        .WriteTo.File("log.text",
                        rollOnFileSizeLimit: true,
                        shared: true,
                        flushToDiskInterval: TimeSpan.FromSeconds(1))
                        .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                            {
                               "image/svg+xml",
                               "application/atom+xml"
                            });
                options.Providers.Add<GzipCompressionProvider>();                
            });
            services.AddMvc(options =>
            {
                //options.OutputFormatters.Add(new VideoOutputFormatter());
            });         

            #region DI
            services.AddOptions();
            services.AddSingleton<ISocketService, SocketService>();
            services.AddTransient<IContentService, ContentService>();
            services.AddSingleton<IUsersManager, UsersManager>();
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddTransient<ICoderSHA, CoderSHA>();
            services.AddTransient<IAutorizationService, AutorizationService>();
            services.AddTransient<IDbConnector, DbConnector>();            
            services.AddTransient<IParserStringService, ParserStringService>();
            services.AddTransient<IConvertManager, ConvertManager>();
            services.AddSingleton<IMiracleService, MiracleService>();
            services.AddSingleton<ICleanContent, CleanContent>();
            

            services.Configure<SQLServer>(opt=> 
            {
                opt.ConnectionStringPG = Configuration["Logging:SQLServer:ConnectionStringPG"];
                opt.QueryPG = Configuration["Logging:SQLServer:QueryPG"];
                opt.QueryTokenPG = Configuration["Logging:SQLServer:QueryTokenPG"];
                opt.ConnectionStringMS = Configuration["Logging:SQLServer:ConnectionStringMS"];
                opt.QueryMS = Configuration["Logging:SQLServer:QueryMS"];
                opt.QueryTokenMS = Configuration["Logging:SQLServer:QueryTokenMS"];
                opt.Provider = (int.Parse(Configuration["Logging:SQLServer:Provider"])==0? Provider.pg: Provider.ms);
                
            });

            services.Configure<Certificate>(opt => 
            {
                opt.Path = Configuration["Logging:Certificate:Path"];
                opt.Key = Configuration["Logging:Certificate:Key"];
            });
            services.Configure<MapCatalog>(opt =>
            {
                opt.Path = Configuration["Logging:MapCatalog:Path"];               
            });
            services.Configure<ImageCatalog>(opt =>
            {
                opt.Path = Configuration["Logging:ImageCatalog:Path"];
            });
            services.Configure<UpdateCatalog>(opt =>
            {
                opt.Path = Configuration["Logging:UpdateCatalog:Path"];
            });
            services.Configure<SocketServer>(opt=> 
            {
                opt.Address = Configuration["Logging:SocketServer:Address"];
                opt.Port = Configuration["Logging:SocketServer:Port"];
                opt.Ws = Configuration["Logging:SocketServer:Ws"];
            });
            services.Configure<Advenced>(opt=> {
                opt.Scheme = Configuration["Logging:Advenced:Scheme"];
                opt.Host = Configuration["Logging:Advenced:Host"];
                opt.Port = Configuration["Logging:Advenced:Port"];
            });
            services.Configure<OperationSystem>(opt=> 
            {
                opt.Platform = Environment.OSVersion.Platform;
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseResponseCompression();
            app.UseMvc(routes=> 
            {
                routes.MapRoute(name: "default", template: "api/v1/{controller=content}/{action=Get}/{key?}/{token?}");
                routes.MapRoute(name: "map", template: "api/v1/{controller=map}/{action=Get}/{z}/{x}/{y}");
                routes.MapRoute(name: "stream", template: "api/v1/{controller=stream}/{action=Get}/{key?}/{token?}");
                routes.MapRoute(name: "update", template: "api/v1/{controller=update}/{action=Get}/{key?}");
            });
                       
            IMiracleService miracleService = app.ApplicationServices.GetService<IMiracleService>();
            miracleService.FillData();
            miracleService.FillTokenCollection();

            ICleanContent cleanContent = app.ApplicationServices.GetService<ICleanContent>();
            cleanContent.Start(60000);
        }
    }
}
