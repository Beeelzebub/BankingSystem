using BankingSystem.Api.Data;
using BankingSystem.Api.Repositories.Contracts;
using BankingSystem.Api.Repositories.Implementations;
using BankingSystem.Api.Services.Contracts;
using BankingSystem.Api.Services.Implementations;
using BankingSystem.Common.HttpClients;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = Configuration["Authority"];
                    options.Audience = Configuration["Audience"];
                    options.RequireHttpsMetadata = false;
                });

            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration["CentralSqlConnectionString"]));
            services.AddHttpClient<JuridicalRegisterHttpClient>();
            services.AddHttpClient<TaxOfficeHttpClient>();
            services.AddHttpClient<UtilityServiceHttpClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
