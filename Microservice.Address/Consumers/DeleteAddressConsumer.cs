using MassTransit;
using MediatR;
using Microservice.Address.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesodev.Core.SharedModels;

namespace Microservice.Address.Consumers
{
    public class DeleteAddressConsumer : IConsumer<DeleteAddressRequestModel>
    {
        private readonly IMediator _mediator;

        public DeleteAddressConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<DeleteAddressRequestModel> context)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var result = await _mediator.Send(new DeleteAddressCommand(context.Message.Id));
                    await context.RespondAsync(new DeleteAddressResponseModel { Message = "Success" });
                }
                catch (Exception ex)
                {
                    await context.RespondAsync(new DeleteAddressResponseModel { Message = ex.Message });
                }
            });
        }
    }
}
