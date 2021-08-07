using MassTransit;
using MediatR;
using Microservice.Product.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesodev.Core.SharedModels.Product.Request;
using Tesodev.Core.SharedModels.Product.Response;

namespace Microservice.Product.Consumers
{
    public class UpdateProductConsumer : IConsumer<UpdateProductRequestModel>
    {
        private readonly IMediator _mediator;

        public UpdateProductConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<UpdateProductRequestModel> context)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var entity = context.Message;
                    var model = new UpdateProductCommand(entity.Id, entity.ImageUrl, entity.Name);

                    var result = await _mediator.Send(model);
                    await context.RespondAsync(new UpdateProductResponseModel { Id = result.Id, Message = "Success" });
                }
                catch (Exception ex)
                {
                    await context.RespondAsync(new UpdateProductResponseModel { Id = 0, Message = ex.Message });
                }
            });
        }    }
}
