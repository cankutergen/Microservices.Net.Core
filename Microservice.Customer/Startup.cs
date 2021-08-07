using FluentValidation;
using Microservice.Customer.DataAccess.Abstract;
using Microservice.Customer.DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microservice.Customer.CQRS;
using Microservice.Customer.Entities.Concrete;
using Microservice.Customer.Business.ValidationRules.FluentValidation;
using MassTransit;
using GreenPipes;
using Microservice.Customer.Consumers;

namespace Microservice.Customer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            using (var client = new CustomerContext())
            {
                var connectionString = this.Configuration.GetConnectionString("CustomerConnection");
                CustomerContext.SetConnectionString(connectionString);
                client.Database.EnsureCreated();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetCustomerConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    config.ReceiveEndpoint("getCustomerQueue", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<GetCustomerConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer.Microservice", Version = "v1" });
            });

            services.AddSingleton<ICustomerDal, EfCustomerDal>();
            services.AddSingleton<IValidator<CustomerModel>, CustomerValidator>();

            services.AddMediatR(typeof(CustomerMediatrEntryPoint).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer.Microservice V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
