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
    public class UpdateAddressConsumer : IConsumer<UpdateAddressRequestModel>
    {
        private readonly IMediator _mediator;

        public UpdateAddressConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<UpdateAddressRequestModel> context)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var entity = context.Message;
                    var model = new UpdateAddressCommand(entity.Id, entity.AddressLine, entity.City, entity.Country, entity.CityCode);

                    var result = await _mediator.Send(model);
                    await context.RespondAsync(new UpdateAddressResponseModel { Id = result.Id, Message = "Success" });
                }
                catch (Exception ex)
                {
                    await context.RespondAsync(new UpdateAddressResponseModel { Id = 0, Message = ex.Message });
                }
            });
        }
    }
}
