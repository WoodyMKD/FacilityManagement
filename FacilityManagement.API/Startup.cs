using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacilityManagement.API.Data;
using FacilityManagement.API.Repositories;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using FacilityManagement.DTOs;
using FacilityManagement.API.Models;

namespace FacilityManagement.API
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
            services.AddMvc(o =>
            {
                /*
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
                */
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IInventoryObjectRepository, InventoryObjectRepository>();
            /*
            services.AddAuthentication(
                IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44301/";
                    options.ApiName = "facilitymanagementapi";
                }
            );
            */

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseAuthentication();
            app.UseStaticFiles();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<InventoryObjectDTO, InventoryObject>()
                    .ForMember(m => m.Types, options => options.Ignore())
                    .ForMember(m => m.InventoryObjectId, options => options.Ignore());

                cfg.CreateMap<InventoryObjectSystemDTO, InventoryObjectSystem>()
                    .ForMember(m => m.InventoryObjectTypeId, options => options.Ignore())
                    .ForMember(m => m.Type, options => options.Ignore())
                    .ForMember(m => m.InventoryObjectSystemId, options => options.Ignore())
                    .ForMember(m => m.Parts, options => options.Ignore());

                cfg.CreateMap<InventoryObjectTypeDTO, InventoryObjectType>()
                    .ForMember(m => m.InventoryObject, options => options.Ignore())
                    .ForMember(m => m.InventoryObjectId, options => options.Ignore())
                    .ForMember(m => m.InventoryObjectTypeId, options => options.Ignore())
                    .ForMember(m => m.Systems, options => options.Ignore());

                cfg.CreateMap<InventoryObjectPartDTO, InventoryObjectPart>()
                    .ForMember(m => m.Systems, options => options.Ignore())
                    .ForMember(m => m.InventoryObjectSystemId, options => options.Ignore())
                    .ForMember(m => m.InventoryObjectPartId, options => options.Ignore());
            });

            Mapper.AssertConfigurationIsValid();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
