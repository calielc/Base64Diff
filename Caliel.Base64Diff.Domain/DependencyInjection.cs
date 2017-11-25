using Caliel.Base64Diff.Domain.Diff;
using Microsoft.Extensions.DependencyInjection;

namespace Caliel.Base64Diff.Domain {
    public sealed class DependencyInjection {
        private DependencyInjection() { }

        public void Resolve(IServiceCollection services) {
            services.AddTransient<IDiffService, DiffService>();
        }

        public static readonly DependencyInjection Instance = new DependencyInjection();
    }
}
