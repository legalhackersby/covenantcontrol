using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using src.Service;
using src.Service.Document;
using src.Service.Upload;
using System.Security.Authentication;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using Quartz.Impl;
using src.Hubs;
using src.Jobs;
using src.Repository;
using src.Service.iSwarm;

namespace src
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                }));
            services.AddSignalR();

            var mongoUrl = Configuration.GetValue<string>("MongoUrl");
            var mongoDatabase = Configuration.GetValue<string>("MongoDatabase");

            services.AddSingleton(_ =>
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(mongoUrl));
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                return new MongoClient(settings);
            });
            services.AddScoped<IMongoDatabase>(x =>
            {
                var client = x.GetRequiredService<MongoClient>();
                IMongoDatabase database = client.GetDatabase(mongoDatabase);
                return database;
            });
            
            services.AddTransient<IMongoDatabase>(x =>
            {
                var client = x.GetRequiredService<MongoClient>();
                IMongoDatabase database = client.GetDatabase(mongoDatabase);
                return database;
            });
            services.AddTransient<IUploadDocumentService, UploadDocumentService>();
            services.AddTransient<IStorage, Storage>();
            services.AddTransient<IStorage, Storage>();
            services.AddSingleton<IConvertToTxt>(new ConvertToTxt());
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<ICovenantSearchStrategy, WordsPercentageMatchCovenantSearchStrategy>();
            services.AddTransient<ITextParserService, TextParserService>();
            services.AddTransient<IWebCrawlerService, WebCrawlerService>();
            services.AddTransient<IChapterMongoRepository, ChapterMongoRepository>();
            services.AddTransient<IChangesSearchResultMongoRepository, ChangesSearchResultMongoRepository>();
            services.AddTransient<ICovenantsWebRepository, CovenantsWebRepository>();
            

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // NOTE: Allowed HTTP and Developer build usage for run for now. Please remove commented below for production.

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }
            /*app.Use(async (context, next) =>
            {
                context.RequestServices
                    .GetRequiredService<IHubContext<NotifyHub>>();
            });*/

            //app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCors("*");
            app.UseCors("CorsPolicy");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotifyHub>("/notify");
            });

            Debugger.Launch();
            Scheduler.Start(app.ApplicationServices.GetService<IWebCrawlerService>());
        }
    }
}
