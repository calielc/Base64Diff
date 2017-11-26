using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Caliel.Base64Diff.Api {
    internal sealed class SwaggerConfig {
        public static readonly SwaggerConfig Instance = new SwaggerConfig();

        private SwaggerConfig() { }

        public void SetupGenerator(IServiceCollection services) {
            services.AddSwaggerGen(SetupGenOptions);
        }

        private static void SetupGenOptions(SwaggerGenOptions options) {
            options.SwaggerDoc("v1", new Info {
                Version = "v1",
                Title = "Base64 Diff API",
                Contact = new Contact {
                    Name = "Caliel Costa",
                    Url = "https://github.com/calielc/Base64Diff",
                    Email = "caliel@calielcosta.com"
                }
            });

            foreach (var xml in new[] { "Caliel.Base64Diff.Api.xml", "Caliel.Base64Diff.Domain.xml" }) {
                options.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, xml));
            }

            options.IgnoreObsoleteProperties();
            options.IgnoreObsoleteActions();
            options.DescribeAllEnumsAsStrings();
        }

        public void SetupResut(IApplicationBuilder app) {
            app.UseSwagger(c => {
                c.RouteTemplate = "{documentName}/manifest.json";
            });

            app.UseSwaggerUI(c => {
                c.DocumentTitle("Base64 Diff API");
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("/v1/manifest.json", "v1");
            });
        }
    }
}