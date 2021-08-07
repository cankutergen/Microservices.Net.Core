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
    public class CreateAddressConsumer : IConsumer<CreateAddressRequestModel>
    {
        private readonly IMediator _mediator;

        public CreateAddressConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<CreateAddressRequestModel> context)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var entity = context.Message;
                    var model = new CreateAddressCommand(entity.AddressLine, entity.City, entity.Country, entity.CityCode);

                    var result = await _mediator.Send(model);
                    await context.RespondAsync(new CreateAddressResponseModel { Id = result.Id, Message = "Success" });
                }
                catch (Exception ex)
                {
                    await context.RespondAsync(new CreateAddressResponseModel { Id = 0, Message = ex.Message });
                }
            });
        }
    }
}
