using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;

namespace SportsStore
{
    public class Startup
    {
        IConfigurationRoot Configuration;

        //adding appsetting.json file (with connection string) to configuration :O
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //DB context using connection string from json file
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:SportStoreProducts:ConnectionString"]));

            //when controller needs IProductRepository, its implementation - EF Product Repository will be delivered
            services.AddTransient<IProductRepository, EFProductRepository>();

            services.AddMvc();

            //            // Build the intermediate service provider then return it
            //            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            //displays errors in very verbose way on webpage
            app.UseDeveloperExceptionPage();

            //404 etc
            app.UseStatusCodePages();

            app.UseStaticFiles();

            //do not use default routing but custom
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Product}/{action=List}/{id?}");
            });

            //populates data
            SeedData.EnsurePopulated(app, context);
        }
    }
}
