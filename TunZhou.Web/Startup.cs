using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TunZhou.Core.Enums;

namespace TunZhou.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public Autofac.IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region HttpClient注入
            services.AddHttpClient(HttpClientConst.TunZhouApi_BaseAddress_CLIENT_NAME, cfg =>
            {
                var baseUrl = Configuration["TuZhouApiBaseAddress"];
                cfg.BaseAddress = new Uri(baseUrl);
                cfg.DefaultRequestHeaders.Add("Accept", "application/json");
                cfg.Timeout = new TimeSpan(0, 0, 0, 20);
            });
            #endregion

            var builder = new ContainerBuilder();
            //注册Service中的对象,Service中的类要以Service结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("TunZhou.Services")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
            //注册Repository中的对象,Repository中的类要以Repository结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("TunZhou.Repositories")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();

            builder.Populate(services);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }
        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="AssemblyName">程序集名称</param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(String AssemblyName)
        {
            return Assembly.Load(AssemblyName);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Test}/{action=Index}/{id?}");
            });
        }
    }
}
