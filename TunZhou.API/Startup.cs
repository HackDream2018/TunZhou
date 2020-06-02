using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using TunZhou.API.Filters;
using TunZhou.Core.Redis;

namespace TunZhou.API
{
    public class Startup
    {
        private readonly IHostingEnvironment Environment;
        private readonly IConfiguration Configuration;

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Environment = env;
            Configuration = configuration;
        }

        public Autofac.IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddSwaggerGen(cfg =>
                {
                    cfg.SwaggerDoc("v1", new Info { Title = "TunZhouAPI", Version = "v1" });
                    var xmlFileName = "TunZhou.API.xml";
                    var xmlFilePath = Path.Combine(Environment.ContentRootPath, xmlFileName);
                    if (File.Exists(xmlFilePath))
                    {
                        cfg.IncludeXmlComments(xmlFilePath);
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            services.AddMemoryCache();//注册缓存服务
            //services.AddMvc(config =>
            //{
            //    config.Filters.Add(new SignVerificationFilter());
            //});//注册过滤器，当注册后所有action都将公用该过滤器
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var builder = new ContainerBuilder();
            //注册Service中的对象,Service中的类要以Service结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("TunZhou.Services")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
            //注册Repository中的对象,Repository中的类要以Repository结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("TunZhou.Repositories")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
            builder.RegisterType<RedisService>().As<IRedisService>().SingleInstance();

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TunZhouAPI V1");
            });

            app.UseMvc();
        }
    }
}
