using KaspiTest.Repository;
using KaspiTest.Services;

namespace KaspiTest.Extension
{
	public static class InfrasctructureExtension
	{
		public static IServiceCollection AddInfrasctructure(this IServiceCollection services)
		{
			services.AddScoped<INewsRespository, NewsRepository>();
			services.AddScoped<IParserService, ParserService>();
			return services;
		}
	}
}
