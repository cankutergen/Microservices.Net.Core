using MassTransit;
using MediatR;
using Microservice.Address.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesodev.Core.SharedModels;

namespace Microservice.Address.Consumers
{
    public class GetAddressConsumer : IConsumer<GetAddressRequestModel>
    {
        private readonly IMediator _mediator;

        public GetAddressConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<GetAddressRequestModel> context)
        {
            await Task.Run(async () =>
            {
                var result = await _mediator.Send(new GetAddresByIdQuery(context.Message.Id));

                var response = new GetAddressResponseModel();
                
                if(result != null)
                {
                    response.Id = result.Id;
                    response.AddressLine = result.AddressLine;
                    response.City = result.City;
                    response.CityCode = result.CityCode;
                    response.Country = result.Country;
                    response.Message = "Success";
                }

                await context.RespondAsync(response);
            });  
        }
    }
}
