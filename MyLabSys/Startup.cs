using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyLabSys.Areas.Paciente.Services;
using MyLabSys.Areas.Paciente.Services.Interfaces;
using MyLabSys.Factories;
using MyLabSys.Factories.Interfaces;
using MyLabSys.Models;
using MyLabSys.Services;
using MyLabSys.Services.Interfaces;
using System;

namespace MyLabSys {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            AdicionarServicoConexaoSqlServer(services);

            RegistrarInterfacesParaInjecaoDependencia(services);

            services.AddControllersWithViews();
        }

        private void RegistrarInterfacesParaInjecaoDependencia(IServiceCollection services) {
            services.AddTransient<IOrdemServicoService, OrdemServicoService>();
            services.AddTransient<IOrdemServicoGridModelFactory, OrdemServicoGridModelFactory>();
            services.AddTransient<IResultadosExamesService, ResultadosExamesService>();
        }

        void AdicionarServicoConexaoSqlServer(IServiceCollection services) {
            services.AddDbContext<MyLabSysContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "PacienteArea",
                    pattern: "{area:exists}/{controller=Paciente}/{action=Index}/{id?}");
            });
        }
    }
}