using backEnd.Interfaces;
using backEnd.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;

namespace backEnd
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
            //string DefaultDatabase = Configuration.GetConnectionString("DefaultDatabase");
            //services.Configure<CadenaConexionSettings>(Configuration.GetSection("DefaultDatabase"));

            //Se agrego el servicio de CORS para permitir la conexion cruzada desde el frontend, se utiliza cuando la solicitud es desde ortro dominio (url).
            services.AddCors();

            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };

            });

            services.AddScoped<IClientes, ClientesServices>();
            services.AddScoped<IArticulo, ArticuloServices>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "backEnd", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Se agrego el servicio de CORS para permitir la conexion cruzada desde el frontend, se utiliza cuando la solicitud es desde ortro dominio (url).
            app.UseCors(options =>
            {
                //options.WithOrigins(""); // Colocar la url completa del frontend con el puerto, por ejemplo: "http://miportal.com:3000"
                options.WithOrigins("*");
                options.AllowAnyMethod();
                options.AllowAnyHeader();
                
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backEnd v1"));
            }

           app.UseHttpsRedirection();

            // Agregar Content/imagenes como carpeta pública
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Content", "imagenes")),
                RequestPath = "/imagenes"
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
