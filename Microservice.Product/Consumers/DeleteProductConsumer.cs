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
    public class DeleteProductConsumer : IConsumer<DeleteProductRequestModel>
    {
        private readonly IMediator _mediator;

        public DeleteProductConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<DeleteProductRequestModel> context)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var result = await _mediator.Send(new DeleteProductCommand(context.Message.Id));
                    await context.RespondAsync(new DeleteProductResponseModel { Message = "Success" });
                }
                catch (Exception ex)
                {
                    await context.RespondAsync(new DeleteProductResponseModel { Message = ex.Message });
                }
            });
        }
    }
}
