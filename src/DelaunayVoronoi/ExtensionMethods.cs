using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DelaunayVoronoi;

public static class ExtensionMethods {

	public static IServiceCollection AddDelauanay(
		this IServiceCollection services
	) {
		services.TryAddSingleton<IDelaunatorFactory, DelaunatorFactory>();
		services.AddSingleton<IDelaunayFactory, DelaunayFactory>();

		return services;
	}

	public static IServiceCollection AddVoronoi(
		this IServiceCollection services
	) {
		services.TryAddSingleton<IDelaunatorFactory, DelaunatorFactory>();
		services.AddSingleton<IVoronoiFactory, VoronoiFactory>();

		return services;
	}
}
