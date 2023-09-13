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
            //Gelen her requestin söyleyeceðimiz bilgilere göre token oluþturmasý gerektiði bilgisini burda veriyoruz.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                // token ý valide edeceðimiz parametlerimizin ne olacaðýný burada giriyoruz.

                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    // Key imzasý olmadan hiçbir þeyi çalýþtýma demek 
                    ValidateIssuerSigningKey = true,

                    // Appsettinge tanýmladýðýmýz security key olarak kullancaðýmýz key imiz burda varmý ve bu symetric key ile mi yapýlmýþ diye baktýðýmýz yer burasý
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),

                    // Bu token e açan kiþiyi valide etmek istermisin istersen True/ istemezsen false  gibi.
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
