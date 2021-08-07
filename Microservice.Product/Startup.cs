using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GreenPipes;
using MassTransit;
using MediatR;
using Microservice.Product.Business.ValidationRules.FluentValidation;
using Microservice.Product.Consumers;
using Microservice.Product.CQRS;
using Microservice.Product.DataAccess.Abstract;
using Microservice.Product.DataAccess.Concrete.EntityFramework;
using Microservice.Product.Entities.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microservice.Product
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            using (var client = new ProductContext())
            {
                var connectionString = this.Configuration.GetConnectionString("ProductConnection");
                ProductContext.SetConnectionString(connectionString);
                client.Database.EnsureCreated();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateProductConsumer>();
                x.AddConsumer<UpdateProductConsumer>();
                x.AddConsumer<GetProductConsumer>();
                x.AddConsumer<DeleteProductConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cur =>
                {
                    cur.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cur.ReceiveEndpoint("createProductQueue", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<CreateProductConsumer>(provider);
                    });

                    cur.ReceiveEndpoint("updateProductQueue", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<UpdateProductConsumer>(provider);
                    });

                    cur.ReceiveEndpoint("getProductQueue", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<GetProductConsumer>(provider);
                    });

                    cur.ReceiveEndpoint("deleteProductQueue", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<DeleteProductConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product.Microservice", Version = "v1" });
            });

            services.AddScoped<IProductDal, EfProductDal>();
            services.AddScoped<IValidator<ProductModel>, ProductValidator>();

            services.AddMediatR(typeof(ProductMediatrEntryPoint).Assembly);
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product.Microservice V1");
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
