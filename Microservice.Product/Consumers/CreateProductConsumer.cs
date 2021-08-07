using MassTransit;
using MediatR;
using Microservice.Product.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesodev.Core.SharedModels.Product;
using Tesodev.Core.SharedModels.Product.Request;
using Tesodev.Core.SharedModels.Product.Response;

namespace Microservice.Product.Consumers
{
    public class CreateProductConsumer : IConsumer<CreateProductRequestModel>
    {
        private readonly IMediator _mediator;

        public CreateProductConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<CreateProductRequestModel> context)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var entity = context.Message;
                    var model = new CreateProductCommand(entity.ImageUrl, entity.Name);

                    var result = await _mediator.Send(model);
                    await context.RespondAsync(new CreateProductResponseModel { Id = result.Id, Message = "Success" });
                }
                catch (Exception ex)
                {
                    await context.RespondAsync(new CreateProductResponseModel { Id = 0, Message = ex.Message });
                }
            });
        }
    }
}
