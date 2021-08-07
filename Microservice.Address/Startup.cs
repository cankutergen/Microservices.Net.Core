using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MassTransit;

using Microservice.Address.CQRS;
using Microservice.Address.DataAccess.Abstract;
using Microservice.Address.DataAccess.Concrete.EntityFramework;
using Microservice.Address.Entities.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microservice.Address.Consumers;
using GreenPipes;
using Microservice.Address.Business.ValidationRules.FluentValidation;

namespace Microservice.Address
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            using (var client = new AddressContext())
            {
                var connectionString = this.Configuration.GetConnectionString("AddressConnection");
                AddressContext.SetConnectionString(connectionString);
                client.Database.EnsureCreated();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateAddressConsumer>();
                x.AddConsumer<UpdateAddressConsumer>();
                x.AddConsumer<GetAddressConsumer>();
                x.AddConsumer<DeleteAddressConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cur =>
                {
                    cur.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cur.ReceiveEndpoint("createAddressQueue", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<CreateAddressConsumer>(provider);
                    });

                    cur.ReceiveEndpoint("updateAddressQueue", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<UpdateAddressConsumer>(provider);
                    });

                    cur.ReceiveEndpoint("getAddressQueue", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<GetAddressConsumer>(provider);
                    });
                    
                    cur.ReceiveEndpoint("deleteAddressQueue", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<DeleteAddressConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Address.Microservice", Version = "v1" });
            });

            services.AddSingleton<IAddressDal, EfAddressDal>();
            services.AddSingleton<IValidator<AddressModel>, AddressValidator>();

            services.AddMediatR(typeof(AddressMediatrEntryPoint).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Address.Microservice V1");
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
