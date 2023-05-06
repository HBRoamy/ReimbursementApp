using Business_Layer.IRepository;
using Business_Layer.Repository;
using Data_Access_Layer.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Shared.User;
using Shared.UserService;
using System.Text;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Reimbursement_Portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; } // use to access appsettings.json

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //configuring jwt, also check appsettings.json
         
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("EasyConnection")));
            services.AddAutoMapper(typeof(Startup));
            services.AddIdentity<Employee, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();  // extenson method of IServiceCollection
            //services.AddScoped<IRepositoryDAL<ReimbursementEntity>, RepositoryDAL<ReimbursementEntity>> (); //configuration doesn't count in coupling
            services.AddScoped<IReimbursementRepo, ReimbursementRepo>();
            services.AddScoped<ICurrentUserInfoService, CurrentUserInfoService>();

            //for setting up cors // also see the Configure() method below he ConfigureServices() method
            services.AddCors(option =>
            {
                option.AddDefaultPolicy(builder =>
                {

                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.Configure<IdentityOptions>(Options =>
            {
                Options.User.RequireUniqueEmail = true;
                Options.Password.RequiredLength = 5;
                Options.Password.RequireDigit = false;
                Options.Password.RequireLowercase = false;
                Options.Password.RequireUppercase = false;
                Options.Password.RequireNonAlphanumeric = true;
                Options.SignIn.RequireConfirmedAccount = false;

            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseCors(options => options.WithOrigins("http://localhost:4200/").AllowAnyMethod());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();
            //app.UseCors(x => x
            //  .AllowAnyMethod()
            //  .AllowAnyHeader()
            //  .SetIsOriginAllowed(origin => true)
            //  .AllowCredentials());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles")),
            //    RequestPath = "/UploadedFiles"
            //});
        }
    }
}
