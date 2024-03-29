﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serko.Common.Middleware;
using Serko.Expense.API.Filters;
using Serko.Expense.API.Library;
using Serko.Expense.API.Services;

namespace Serko.Expense.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(o => 
            {
                o.DefaultScheme = Constants.AUTHENTICATION_SCHEME;
            })
            .AddScheme<ApiKeyOptions, ApiKeyAuthenticationHandler>(Constants.AUTHENTICATION_SCHEME, o => { });

            services.AddScoped<Interfaces.IApiAuthenticationService, ApiAuthenticationService>();
            services.AddScoped<Interfaces.IValidator, Validator>();
            services.AddScoped<Interfaces.IParser, Parser>();
            services.AddSingleton<Common.Log.ILogger, Common.Log.Logger>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMvc();

            //app.Use(async (context, next) =>
            //{
            //    var sw = new Stopwatch();
            //    sw.Start();
            //    await next.Invoke();
            //    sw.Stop();
            //    await context.Response.WriteAsync($"<!-- {sw.ElapsedMilliseconds} ms -->");
            //});

            app.Run(async context =>
            {
                await context.Response.WriteAsync(@"
                                    Following are available Actions.
                                    1. /api/v1/payment/expense/import is a POST method, parameters: ExpenseRequest");
            });
        }
    }
}
