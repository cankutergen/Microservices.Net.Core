using MassTransit;
using MediatR;
using Microservice.Product.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesodev.Core.SharedModels.Product.Request;
using Tesodev.Core.SharedModels.Product.Response;

namespace Microservice.Product.Consumers
{
    public class GetProductConsumer : IConsumer<GetProductRequestModel>
    {
        private readonly IMediator _mediator;

        public GetProductConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<GetProductRequestModel> context)
        {
            await Task.Run(async () =>
            {
                var result = await _mediator.Send(new GetProductByIdQuery(context.Message.Id));
                var response = new GetProductResponseModel();
                
                if(result != null)
                {
                    response.Id = result.Id;
                    response.ImageUrl = result.ImageUrl;
                    response.Name = result.Name;
                    response.Message = "Success";
                }

                await context.RespondAsync(response);
            });
        }
    }
}
