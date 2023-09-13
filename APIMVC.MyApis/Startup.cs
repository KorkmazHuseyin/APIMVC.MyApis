using APIMVC.MyApis.DAL.AuthDAL.Concrete;
using APIMVC.MyApis.DAL.AuthDAL.Interfaces;
using APIMVC.MyApis.DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMVC.MyApis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }





        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIMVC.MyApis", Version = "v1" });
            });
            ///////////////////////////////////////////////////////////////////////////
            services.AddDbContext<MyContext>(a => a.UseSqlServer("server=KORKMAZ\\MSSQLSERVER04;Database=NORTHWND;uid=sa;pwd=1 "));
            services.AddDbContext<AuthContext>(a => a.UseSqlServer("server=KORKMAZ\\MSSQLSERVER04;Database=NORTHWND;uid=sa;pwd=1 "));


            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IAuthDAL, AuthDAL>();
           

            services.AddAuthorization();
            //Gelen her requestin s�yleyece�imiz bilgilere g�re token olu�turmas� gerekti�i bilgisini burda veriyoruz.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                // token � valide edece�imiz parametlerimizin ne olaca��n� burada giriyoruz.

                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    // Key imzas� olmadan hi�bir �eyi �al��t�ma demek 
                    ValidateIssuerSigningKey = true,

                    // Appsettinge tan�mlad���m�z security key olarak kullanca��m�z key imiz burda varm� ve bu symetric key ile mi yap�lm�� diye bakt���m�z yer buras�
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),

                    // Bu token e a�an ki�iyi valide etmek istermisin istersen True/ istemezsen false  gibi.
                    ValidateIssuer = false
                };
            });

            ///////////////////////////////////////////////////////////////////////
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIMVC.MyApis v1"));

            }
           


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
