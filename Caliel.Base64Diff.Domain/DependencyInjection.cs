using System.IO;
using Caliel.Base64Diff.Data;
using Caliel.Base64Diff.Domain.Diff;
using Microsoft.Extensions.DependencyInjection;

namespace Caliel.Base64Diff.Domain {
    public sealed class DependencyInjection {
        public static readonly DependencyInjection Instance = new DependencyInjection();

        private DependencyInjection() { }

        public void Resolve(IServiceCollection services, Config config) {
            services.AddSingleton<IDiffContentRepository, DiffContentFS>(_ => new DiffContentFS(Path.Combine(config.BasePath, "diff")));

            services.AddTransient<IDiffService, DiffService>();
        }

        public struct Config {
            public string BasePath { get; set; }
        }
    }
}
