using BankingSystem.Common.HttpClients;
using BankingSystem.TaxOffice.Api.Data;
using BankingSystem.TaxOffice.Api.Repositories.Contracts;
using BankingSystem.TaxOffice.Api.Repositories.Implementations;
using BankingSystem.TaxOffice.Api.Services.Contracts;
using BankingSystem.TaxOffice.Api.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.TaxOffice.Api
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

            services.AddScoped<ITransferTaxRepository, TransferTaxRepository>();
            services.AddScoped<ITransferTaxService, TransferTaxService>();
            services.AddDbContext<TaxOfficeContext>(options => options.UseSqlServer(Configuration["TaxOfficeSqlConnectionString"]));
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
