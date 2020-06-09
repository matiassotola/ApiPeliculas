using ApiPeliculas.Data;
using ApiPeliculas.Helpers;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace ApiPeliculas
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Cada vez que agreguemos un nuevo repositorio tenemos que agregarlo aqu�.
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IPeliculaRepository, PeliculaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // El AutoMapper nos permite mapear de una clase a otra.            
            services.AddAutoMapper(typeof(PeliculasMappers));

            //Agregamos la dependencia del Token.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //De aqu� en adelante configuraci�n de documentaci�n de nuestra API.
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiPeliculasCategorias", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API Categor�as Pel�culas",
                    Version = "1",
                    Description = "Backend Pel�culas",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "matgas@gmail.com",
                        Name = "matgas",
                        Url = new Uri("https://apipeliculas.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                options.SwaggerDoc("ApiPeliculas", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API Pel�culas",
                    Version = "1",
                    Description = "Backend Pel�culas",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "matgas@gmail.com",
                        Name = "matgas",
                        Url = new Uri("https://apipeliculas.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                options.SwaggerDoc("ApiPeliculasUsuarios", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API Usuarios Pel�culas",
                    Version = "1",
                    Description = "Backend Pel�culas",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "matgas@gmail.com",
                        Name = "matgas",
                        Url = new Uri("https://apipeliculas.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);
                options.IncludeXmlComments(rutaApiComentarios);

                //Primero vamos a definir el esquema que vamos a utilizar que es el JWT.
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Autentizaci�n JWT (Bearer)",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });
                
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme { 
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });                
            });

            services.AddControllers();

            //Damos soporte para CORS
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Validamos si estamos trabajando en un entorno de desarrollo.
            //Cuando estamos en un entorno de Desarrollo nos muetra el siguiente condicional.
            //Nos da mas detalle del error para el desarrollador.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Cuando no estamos en desarrollo, sino en producci�n se ejecuta las siguientes l�neas.
                //Vamos acceder al contexto, al Request y Response del HTTP.
                //Desde aqu� estamos accediendo pero como es en el Startup se agrega de manera global, si se
                //detecta alguna excepci�n no controlada aqu� la podemos capturar.
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            // Llamamos al m�todo de la clase Extensions.cs que creamos
                            context.Response.AddAplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseHttpsRedirection();

            //L�nea para documentaci�n API
            app.UseSwagger();

            //Se utliza para ingresar directamente a la ruta http://localhost/swagger/index.html
            app.UseSwaggerUI(options =>
            {
                /*DESARROLLO*/

                //options.SwaggerEndpoint("/swagger/ApiPeliculasCategorias/swagger.json", "Api Categor�as Pel�culas");
                //options.SwaggerEndpoint("/swagger/ApiPeliculas/swagger.json", "Api Pel�culas");
                //options.SwaggerEndpoint("/swagger/ApiPeliculasUsuarios/swagger.json", "Api Usuarios Pel�culas");

                /*PRODUCCI�N*/

                options.SwaggerEndpoint("/swagger/ApiPeliculasCategorias/swagger.json", "Api Categor�as Pel�culas");
                options.SwaggerEndpoint("/swagger/ApiPeliculas/swagger.json", "Api Pel�culas");
                options.SwaggerEndpoint("/swagger/ApiPeliculasUsuarios/swagger.json", "Api Usuarios Pel�culas");

                // Colocamos la siguiente instrucci�n para que no de error al cargar la pagina (HTTP ERROR 404) 
                // luedo de eliminar la l�nea "launchUrl" de la secci�n profiles del archivo launchSettings.json.
                options.RoutePrefix = "";
            });

            app.UseRouting();

            // Estos dos son para la autenticaci�n y autorizaci�n
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Damos soporte para CORS
            //A modo de prueba permitimos conectarnos a esta API desde cualquier origen, 
            //que permita cualquier m�todo, que permita cualquier encabezado. 
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
