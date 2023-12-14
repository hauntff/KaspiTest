using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KaspiTest.Extension
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            // JWT Bearer authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["Auth:Issuer"],
						ValidateAudience = true,
						ValidAudience = configuration["Auth:Audience"],
						ValidateLifetime = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Auth:Key"] ?? "")),
						ValidateIssuerSigningKey = true,
					};
				});
			return services;
        }
    }
}
