
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;
using TalentTrail.Models;
using TalentTrail.Services;

namespace TalentTrail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(x=>
             x.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.Preserve);


            builder.Services.AddScoped<ISignUpService, SignUpService>();
            builder.Services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();
            builder.Services.AddScoped<IEmployerProfileService, EmployerProfileService>();
            builder.Services.AddScoped<IJobPostService, JobPostService>();
            builder.Services.AddScoped<IJobSeekerService, JobSeekerService>();
            builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
            builder.Services.AddScoped<IResumeService, ResumeService>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();

            // builder.Services.AddSwaggerGen(); 
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
                //opt.SchemaFilter<EnumSchemaFilter>();
            });

            // Configure JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, // Set to true to validate the token's expiration time
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:3000") // Replace with your frontend URL
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials(); // Allow credentials (cookies, authorization headers)
                    });
            });

            builder.Services.AddDbContext<TalentTrailDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("conStr")));

            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository,new FileInfo("log4net.config"));


            
            var app = builder.Build();
            app.UseCors("AllowSpecificOrigin");
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
 


            app.MapControllers();

            app.Run();
        }
    }
}
