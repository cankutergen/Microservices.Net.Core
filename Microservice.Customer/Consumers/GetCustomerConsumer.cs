using MassTransit;
using MediatR;
using Microservice.Customer.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesodev.Core.SharedModels.Customer.Request;
using Tesodev.Core.SharedModels.Customer.Response;

namespace Microservice.Customer.Consumers
{
    public class GetCustomerConsumer : IConsumer<GetCustomerRequestModel>
    {
        private readonly IMediator _mediator;

        public GetCustomerConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<GetCustomerRequestModel> context)
        {
            await Task.Run(async () =>
            {
                var result = await _mediator.Send(new GetCustomerByIdQuery(context.Message.Id));
                var response = new GetCustomerResponseModel();

                if(result != null)
                {
                    response.Id = result.Id;
                    response.AddressId = result.AddressId;
                    response.CreatedAt = result.CreatedAt;
                    response.Email = result.Email;
                    response.Name = result.Name;
                    response.UpdatedAt = result.UpdatedAt;
                    response.Message = "Success";
                }

                await context.RespondAsync(response);
            });
        }
    }
}
