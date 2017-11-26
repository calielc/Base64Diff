using Microsoft.AspNetCore.Builder;
using Caliel.Base64Diff.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Caliel.Base64Diff.Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            DependencyInjection.Instance.Resolve(services, new DependencyInjection.Config {
                BasePath = "c:\\Temp"
            });

            SwaggerConfig.Instance.SetupGenerator(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            SwaggerConfig.Instance.SetupResut(app);
        }
    }
}
